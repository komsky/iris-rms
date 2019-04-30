using Iris.Rms.Data;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Iris.Rms.Web.Host.Controllers
{
    public class RmsBaseController : Controller
    {
        protected readonly RmsDbContext _context;

        public RmsBaseController(RmsDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
    }
}
