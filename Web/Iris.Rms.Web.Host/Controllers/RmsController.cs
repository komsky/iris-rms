using Iris.Rms.Data;
using Iris.Rms.Interfaces;
using Iris.Rms.Models;
using Iris.Rms.Web.Host.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                .ThenInclude(device=>device.Hooks).ToList();
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
                return _context.RmsList.Include(rms=>rms.Devices).Single(rms => rms.RmsId == rmsId).Devices.ToList();
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
                return _context.Devices.Include(device=>device.Hooks).Single(device => device.RmsDeviceId == deviceId);

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
                _context.Devices.Include(device=>device.Hooks).Single(device => device.RmsDeviceId == deviceId).Hooks.Add(hook);
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
                var thisWebHook =  _context.WebHooks.Single(webHook => webHook.WebHookId == webHookId);
                var response =  await _rmsService.ActOnWebHook(thisWebHook);
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

                var device = _context.Devices.SingleOrDefault(x => x.MAC.Replace(":", "").ToLower() == deviceMac.Replace(":", "").ToLower());
                if (device == null)
                {
                    _context.RmsList.First().Devices.Add(new RmsDevice { MAC = ToRegularMac(deviceMac), IpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString() });
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

        private string ToRegularMac(string deviceMac)
        {
            if (string.IsNullOrEmpty(deviceMac))
            {
                throw new ArgumentNullException(deviceMac);
            }
            if (deviceMac.Contains(":")) return deviceMac;

            var macAddress = new StringBuilder();
            for (int i = 0; i < deviceMac.Length; i++)
            {
                macAddress.Append(deviceMac[i].ToString().ToUpper());
                i++;
                macAddress.Append(deviceMac[i].ToString().ToUpper());
                macAddress.Append(":");
            }
            return macAddress.ToString();
        }
    }
}
