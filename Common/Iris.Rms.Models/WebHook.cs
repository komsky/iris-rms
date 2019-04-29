using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Iris.Rms.Models
{
    public class WebHook
    {
        public int WebHookHookId { get; set; }

        public Uri HookUrl { get; set; }
        public HttpMethod Method { get; set; }
        public string Body { get; set; }

        public int RmsId { get; set; }
        //add some kind of trigger
    }
}
