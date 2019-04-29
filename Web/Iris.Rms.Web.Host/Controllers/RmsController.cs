using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iris.Rms.Data;
using Iris.Rms.Models;
using Microsoft.AspNetCore.Mvc;

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


    }
}
