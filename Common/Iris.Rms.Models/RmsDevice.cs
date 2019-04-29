using Iris.Rms.Models.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Iris.Rms.Models
{
    public class RmsDevice
    {
        public RmsDevice()
        {
            Hooks = new List<WebHook>();
        }
        [Key]
        public int RmsDeviceId { get; set; }
        public DeviceType Type { get; set; }
        public string TypeDescription { get { return Type.ToString(); } }
        public string Location { get; set; }
        public string MAC { get; set; }
        public string IpAddress { get; set; }
        public string CurrentStatus { get; set; }
        public virtual ICollection<WebHook> Hooks { get; set; }

    }
}
