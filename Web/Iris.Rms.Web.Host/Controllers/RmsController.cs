using Iris.Rms.Data;
using Iris.Rms.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Iris.Rms.Web.Host.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RmsController : RmsBaseController
    {
        public RmsController(RmsDbContext context) : base(context)
        {
        }

        [HttpGet]
        public ActionResult<IEnumerable<RmsConfig>> Get()
        {
            return _context.RmsList;
        }

        [HttpGet]
        [Route("{rmsId}")]
        public ActionResult<IEnumerable<RmsConfig>> GetRms(int rmsId)
        {
            return _context.RmsList;
        }

        [HttpGet]
        [Route("{rmsId}/device")]
        public ActionResult<RmsConfig> GetDevices(int rmsId)
        {
            try
            {
                return _context.RmsList.Single(rms => rms.RmsId == rmsId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
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


        [HttpPost]
        [Route("{rmsId}/device/{deviceId}")]
        public ActionResult<RmsDevice> GetDeviceDetails(int rmsId, int deviceId)
        {
            try
            {
                return _context.Devices.Single(device => device.RmsDeviceId == deviceId);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
