using Newtonsoft.Json;
using System.Collections.Generic;

namespace Iris.Rms.Web.Host.Models
{
    public class RmsDeviceInterface
    {
        public string name { get; set; }
        public string location { get; set; }
        public string macAddress { get; set; }
        public string type { get; set; }
        public string localIp { get; set; }
        public string uptime { get; set; }
    }

    public class RmsNode
    {
        public string name { get; set; }
        public string location { get; set; }
        public string macAddress { get; set; }
        public List<RmsNodeDevice> devices { get; set; }
    }

    public class RmsNodeDevice
    {
        public string name { get; set; }
        public string type { get; set; }
        public string macAddress { get; set; }
        public string status { get; set; }

    }

    public class RmsConfigModel
    {
        [JsonProperty("interface")]
        public RmsDeviceInterface RmsDeviceInterface { get; set; }
        public List<RmsNode> nodes { get; set; }
    }
}
