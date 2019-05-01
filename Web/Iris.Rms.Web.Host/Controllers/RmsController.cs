using Iris.Rms.Data;
using Iris.Rms.Interfaces;
using Iris.Rms.Models;
using Iris.Rms.Models.Enums;
using Iris.Rms.Web.Host.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Iris.Rms.Web.Host.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RmsController : RmsBaseController
    {
        private IRmsService _rmsService;

        public RmsController(RmsDbContext context, IRmsService rmsService) : base(context)
        {
            _rmsService = rmsService ?? throw new ArgumentNullException(nameof(rmsService));
        }

        [HttpGet]
        public ActionResult<IEnumerable<RmsConfig>> Get()
        {
            return _context.RmsList
                .Include(rms => rms.Hvacs)
                .Include(rms => rms.Lights)
                .ThenInclude(device => device.Hooks).ToList();
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> PostHeartbeat(RmsConfigModel model)
        {
            if (model == null)
            {
                return new ApiResponse { Status = Status.Error, Message = "model was empty" };
            }

            try
            {
                await FlushDatabase();
                await CreateDbFromModel(model);
                return new ApiResponse { Status = Status.Success, Message = "Device db recreated sucessfully" };

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse { Status = Status.Error, Message = ex.Message });

            }
        }

        private async Task CreateDbFromModel(RmsConfigModel model)
        {
            RmsConfig rms = new RmsConfig { Name = model.RmsDeviceInterface.name, Location = model.RmsDeviceInterface.location, IpAddress = model.RmsDeviceInterface.localIp, MacAddress = model.RmsDeviceInterface.macAddress };

            foreach (RmsNode node in model.nodes)
            {
                foreach (RmsNodeDevice nodeDevice in node.devices)
                {
                    DeviceType deviceType = ParseLightType(nodeDevice.type);
                    if (deviceType == DeviceType.LightOnOff)
                    {
                        Light newLight = new Light
                        {
                            Location = node.location,
                            Type = deviceType,
                            MAC = nodeDevice.macAddress,
                            CurrentStatus = nodeDevice.status
                        };

                        newLight.Hooks.Add(new WebHook { ActivationCommand = RmsCommand.LightsOn, HookUrl = BuildHookUrl(nodeDevice, model.RmsDeviceInterface.localIp), Method = "POST", Body = BuildHookActiveBody(node, newLight) });
                        newLight.Hooks.Add(new WebHook { ActivationCommand = RmsCommand.LightsOff, HookUrl = BuildHookUrl(nodeDevice, model.RmsDeviceInterface.localIp), Method = "POST", Body = BuildHookInactiveBody(node, newLight) });
                        rms.Lights.Add(newLight);
                    }

                    if (deviceType == DeviceType.Hvac)
                    {
                        Hvac newHvac = new Hvac
                        {
                            Location = node.location,
                            MAC = nodeDevice.macAddress
                        };
                        double currentTemp;
                        if (double.TryParse(nodeDevice.currentTemperature, out currentTemp))
                        {
                            newHvac.CurrentTemperature = currentTemp;
                        }
                        double setpointTemp;
                        if (double.TryParse(nodeDevice.setTemperature, out setpointTemp))
                        {
                            newHvac.SetpointTemperature = setpointTemp;
                        }
                        newHvac.Hooks.Add(new WebHook { ActivationCommand = RmsCommand.HvacSetTemperature, HookUrl = BuildHookUrl(nodeDevice, model.RmsDeviceInterface.localIp), Method = "POST", Body = BuildHookSetTemperatureBody(node, newHvac) });
                        rms.Hvacs.Add(newHvac);
                    }
                }
            }
            _context.RmsList.Add(rms);
            _context.SaveChanges();
        }

        private DeviceType ParseLightType(string type)
        {
            switch (type)
            {
                case "light_nondimmable":
                    return DeviceType.LightOnOff;
                case "light_dimmable":
                    return DeviceType.LightPercentage;
                case "thermostat":
                    return DeviceType.Hvac;
                default:
                    return DeviceType.LightOnOff;
            }
        }

        private async Task FlushDatabase()
        {
            await _context.Database.ExecuteSqlCommandAsync(new RawSqlString("execute sp_TruncateDatabase"));
        }
        private string BuildHookSetTemperatureBody(RmsNode node, Hvac hvac)
        {
            List<RmsNode> nodes = GetBodySetTemperature(node, hvac);
            return JsonConvert.SerializeObject(new { nodes });
        }

        private string BuildHookActiveBody(RmsNode node, Light device)
        {
            List<RmsNode> nodes = GetBodyStatusOn(node, device);
            return JsonConvert.SerializeObject(new { nodes });
        }
        private string BuildHookInactiveBody(RmsNode node, Light device)
        {
            List<RmsNode> nodes = GetBodyStatusOff(node, device);
            return JsonConvert.SerializeObject(new { nodes });
        }

        private static List<RmsNode> GetBodyStatusOn(RmsNode node, Light device)
        {
            return new List<RmsNode> { new RmsNode { macAddress = node.macAddress, devices = new List<RmsNodeDevice> { new RmsNodeDevice { macAddress = device.MAC, status = ((int)GenericStatus.On).ToString() } } } };
        }
        private static List<RmsNode> GetBodySetTemperature(RmsNode node, Hvac device)
        {
            return new List<RmsNode> { new RmsNode { macAddress = node.macAddress, devices = new List<RmsNodeDevice> { new RmsNodeDevice { macAddress = device.MAC, setTemperature = "{setTemperature}" } } } };
        }
        private static List<RmsNode> GetBodyStatusOff(RmsNode node, Light device)
        {
            return new List<RmsNode> { new RmsNode { macAddress = node.macAddress, devices = new List<RmsNodeDevice> { new RmsNodeDevice { macAddress = device.MAC, status = ((int)GenericStatus.Off).ToString() } } } };
        }

        private string BuildHookUrl(RmsNodeDevice nodeDevice, string ipAddress)
        {
            return new Uri($"http://{ipAddress}/api/status").ToString();
        }

        private RmsCommand GetActivationCommand(string type)
        {
            switch (type)
            {
                case "light_nondimmable":
                    return RmsCommand.LightsOn;
                default:
                    return RmsCommand.LightsOn;
            }
        }
        private RmsCommand GetDeactivationCommand(string type)
        {
            switch (type)
            {
                case "light_nondimmable":
                    return RmsCommand.LightsOff;
                default:
                    return RmsCommand.LightsOff;
            }
        }

        private void UpdateDevice(RmsConfigModel model)
        {
            Light device = _context.Lights.SingleOrDefault(rmsDevice => rmsDevice.MAC == model.RmsDeviceInterface.macAddress);
            foreach (RmsNode node in model.nodes)
            {
                foreach (RmsNodeDevice nodeDevice in node.devices)
                {
                    //TODO: update hooks here
                }
            }
            _context.SaveChanges();
        }

        [HttpGet]
        [Route("{rmsId}")]
        public ActionResult<RmsConfig> GetRms(int rmsId)
        {
            try
            {
                return _context.RmsList.Include(rms => rms.Lights).Single(rms => rms.RmsId == rmsId);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse { Status = Status.Error, Message = ex.Message });
            }
        }

        [HttpGet]
        [Route("{rmsId}/device")]
        public ActionResult<IEnumerable<Light>> GetDevices(int rmsId)
        {
            try
            {
                return _context.RmsList.Include(rms => rms.Lights).Single(rms => rms.RmsId == rmsId).Lights.ToList();
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse { Status = Status.Error, Message = ex.Message });
            }
        }
        [HttpGet]
        [Route("{rmsId}/device/{deviceId}")]
        public ActionResult<Light> GetDeviceDetails(int rmsId, int deviceId)
        {
            try
            {
                return _context.Lights.Include(device => device.Hooks).Single(device => device.RmsDeviceId == deviceId);

            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse { Status = Status.Error, Message = ex.Message });
            }
        }
        [HttpPost]
        [Route("{rmsId}/device")]
        public ActionResult<Light> PostRegisterDevice(Light device)
        {
            _context.Lights.Add(device);
            _context.SaveChanges();
            return device;
        }

        [HttpGet]
        [Route("{rmsId}/device/{deviceId}/webhooks")]
        public ActionResult<Light> GetDeviceWebHooks(int rmsId, int deviceId)
        {
            try
            {
                return _context.Lights.Include(device => device.Hooks).Single(device => device.RmsDeviceId == deviceId);

            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse { Status = Status.Error, Message = ex.Message });
            }
        }
        [HttpPost]
        [Route("{rmsId}/device/{deviceId}/webhooks")]
        public ActionResult<IEnumerable<WebHook>> RegisterDeviceWebHook(int rmsId, int deviceId, [FromBody] WebHook hook)
        {
            try
            {
                _context.Lights.Include(device => device.Hooks).Single(device => device.RmsDeviceId == deviceId).Hooks.Add(hook);
                _context.SaveChanges();
                return _context.Lights.Include(device => device.Hooks).Single(device => device.RmsDeviceId == deviceId).Hooks.ToList();

            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse { Status = Status.Error, Message = ex.Message });
            }
        }
        [HttpGet]
        [Route("{rmsId}/device/{deviceId}/webhooks/{webHookId}")]
        public ActionResult<WebHook> WebhookDetails(int webHookId)
        {
            try
            {
                return _context.WebHooks.Single(webHook => webHook.WebHookId == webHookId);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse { Status = Status.Error, Message = ex.Message });
            }
        }
        [HttpGet]
        [Route("{rmsId}/device/{deviceId}/webhooks/{webHookId}/act")]
        public async Task<ActionResult<ApiResponse>> ActOnWebhook(int deviceId, int webHookId)
        {
            try
            {
                WebHook thisWebHook = _context.Lights.Single(dev => dev.RmsDeviceId == deviceId).Hooks.Single(webHook => webHook.WebHookId == webHookId);
                System.Net.Http.HttpResponseMessage response = await _rmsService.ActOnWebHook(thisWebHook);
                return new ApiResponse { Status = Status.Success, Message = await response.Content.ReadAsStringAsync() };
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse { Status = Status.Error, Message = ex.Message });
            }
        }

        [HttpPost]
        [Route("device/{deviceMac}/{status}")]
        public async Task<ActionResult<ApiResponse>> DeviceStatusChange(string deviceMac, string status)
        {
            try
            {
                if (string.IsNullOrEmpty(deviceMac))
                {
                    throw new ArgumentNullException(nameof(deviceMac));
                }
                if (string.IsNullOrEmpty(status))
                {
                    throw new ArgumentNullException(nameof(status));
                }

                Light device = _context.Lights.SingleOrDefault(x => x.MAC == deviceMac);
                if (device == null)
                {
                    _context.RmsList.First().Lights.Add(new Light { MAC = deviceMac, IpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString() });
                    return new ApiResponse { Status = Status.Success, Message = "Device successfully created" };
                }
                else
                {

                    return new ApiResponse { Status = Status.Success, Message = "Device status successfully updated" };
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse { Status = Status.Error, Message = ex.Message });
            }
        }


    }
}
