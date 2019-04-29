using Iris.Rms.Data;
using Iris.Rms.Interfaces;
using Iris.Rms.Models;
using Iris.Rms.Models.Enums;
using Iris.Rms.Web.Host.Helpers;
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
                .Include(rms => rms.Devices)
                .ThenInclude(device => device.Hooks).ToList();
        }

        [HttpPost]
        public ActionResult<ApiResponse> PostHeartbeat(RmsConfigModel model)
        {
            if (model == null)
            {
                return new ApiResponse { Status = Status.Error, Message = "model was empty" };
            }

            try
            {
                if (_context.Devices.Any(device => device.IpAddress == model.RmsDeviceInterface.localIp))
                {
                    UpdateDevice(model);
                    return new ApiResponse { Status = Status.Success, Message = "Device updated successfully - FAKE" };
                }
                else
                {
                    CreateDevice(model);
                    return new ApiResponse { Status = Status.Success, Message = "Device created successfully - FAKE" };
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse { Status = Status.Error, Message = ex.Message });

            }
        }

        private void CreateDevice(RmsConfigModel model)
        {
            RmsDevice device = new RmsDevice { IpAddress = model.RmsDeviceInterface.localIp, Location = model.RmsDeviceInterface.location, MAC = model.RmsDeviceInterface.macAddress };
            foreach (RmsNode node in model.nodes)
            {
                foreach (RmsNodeDevice nodeDevice in node.devices)
                {
                    device.Hooks.Add(new WebHook { ActivationCommand = GetActivationCommand(nodeDevice.type), HookUrl = BuildHookUrl(nodeDevice, device), Method = "POST", Body = BuildHookActiveBody(nodeDevice, device) });
                    device.Hooks.Add(new WebHook { ActivationCommand = GetDeactivationCommand(nodeDevice.type), HookUrl = BuildHookUrl(nodeDevice, device), Method = "POST", Body = BuildHookInactiveBody(nodeDevice, device) });
                }
            }
            _context.Devices.Add(device);
            _context.SaveChanges();
        }

        private string BuildHookActiveBody(RmsNodeDevice nodeDevice, RmsDevice device)
        {
            List<RmsNode> body = GetBodyStatusOn(nodeDevice, device);
            return JsonConvert.SerializeObject(body);
        }
        private string BuildHookInactiveBody(RmsNodeDevice nodeDevice, RmsDevice device)
        {
            List<RmsNode> body = GetBodyStatusOff(nodeDevice, device);
            return JsonConvert.SerializeObject(body);
        }

        private static List<RmsNode> GetBodyStatusOn(RmsNodeDevice nodeDevice, RmsDevice device)
        {
            return new List<RmsNode> { new RmsNode { macAddress = device.MAC, devices = new List<RmsNodeDevice> { new RmsNodeDevice { macAddress = nodeDevice.macAddress, status = ((int)GenericStatus.On).ToString() } } } };
        }
        private static List<RmsNode> GetBodyStatusOff(RmsNodeDevice nodeDevice, RmsDevice device)
        {
            return new List<RmsNode> { new RmsNode { macAddress = device.MAC, devices = new List<RmsNodeDevice> { new RmsNodeDevice { macAddress = nodeDevice.macAddress, status = ((int)GenericStatus.Off).ToString() } } } };
        }

        private string BuildHookUrl(RmsNodeDevice nodeDevice, RmsDevice device)
        {
            return new Uri($"http://{device.IpAddress}/api/status").ToString();
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
            RmsDevice device = _context.Devices.Single(rmsDevice => rmsDevice.MAC.ToUrlEncodedMac() == model.RmsDeviceInterface.macAddress.ToUrlEncodedMac());
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
                return _context.RmsList.Include(rms => rms.Devices).Single(rms => rms.RmsId == rmsId);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse { Status = Status.Error, Message = ex.Message });
            }
        }

        [HttpGet]
        [Route("{rmsId}/device")]
        public ActionResult<IEnumerable<RmsDevice>> GetDevices(int rmsId)
        {
            try
            {
                return _context.RmsList.Include(rms => rms.Devices).Single(rms => rms.RmsId == rmsId).Devices.ToList();
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse { Status = Status.Error, Message = ex.Message });
            }
        }
        [HttpGet]
        [Route("{rmsId}/device/{deviceId}")]
        public ActionResult<RmsDevice> GetDeviceDetails(int rmsId, int deviceId)
        {
            try
            {
                return _context.Devices.Include(device => device.Hooks).Single(device => device.RmsDeviceId == deviceId);

            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse { Status = Status.Error, Message = ex.Message });
            }
        }
        [HttpPost]
        [Route("{rmsId}/device")]
        public ActionResult<RmsDevice> PostRegisterDevice(RmsDevice device)
        {
            _context.Devices.Add(device);
            _context.SaveChanges();
            return device;
        }

        [HttpGet]
        [Route("{rmsId}/device/{deviceId}/webhooks")]
        public ActionResult<RmsDevice> GetDeviceWebHooks(int rmsId, int deviceId)
        {
            try
            {
                return _context.Devices.Include(device => device.Hooks).Single(device => device.RmsDeviceId == deviceId);

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
                _context.Devices.Include(device => device.Hooks).Single(device => device.RmsDeviceId == deviceId).Hooks.Add(hook);
                _context.SaveChanges();
                return _context.Devices.Include(device => device.Hooks).Single(device => device.RmsDeviceId == deviceId).Hooks.ToList();

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
        public async Task<ActionResult<ApiResponse>> ActOnWebhook(int webHookId)
        {
            try
            {
                WebHook thisWebHook = _context.WebHooks.Single(webHook => webHook.WebHookId == webHookId);
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

                RmsDevice device = _context.Devices.SingleOrDefault(x => x.MAC.ToUrlEncodedMac() == deviceMac.ToUrlEncodedMac());
                if (device == null)
                {
                    _context.RmsList.First().Devices.Add(new RmsDevice { MAC = deviceMac.FromUrlDecodedMac(), IpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString() });
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
