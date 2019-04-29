using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iris.Rms.Data;
using Iris.Rms.Web.Host.Models;
using Microsoft.AspNetCore.Mvc;

namespace Iris.Rms.Web.Host.Controllers
{
    public class MainController : RmsBaseController
    {
        public MainController(RmsDbContext context) : base(context)
        {
        }

        [Route("/")]
        [HttpGet]
        public ActionResult<ApiResponse> Get()
        {
            return new ApiResponse { Status = Status.Success, Message = $"All good. We have {_context.Devices.Count()} devices configured" };
        }
    }
}
