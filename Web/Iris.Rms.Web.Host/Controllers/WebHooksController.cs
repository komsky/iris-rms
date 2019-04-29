using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iris.Rms.Data;

namespace Iris.Rms.Web.Host.Controllers
{
    public class WebHooksController : RmsBaseController
    {
        public WebHooksController(RmsDbContext context) : base(context)
        {
        }

        
    }
}
