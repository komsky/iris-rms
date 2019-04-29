using Iris.Rms.Models.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Iris.Rms.Models
{
    public class RmsDevice
    {
        [Key]
        public int RmsDeviceId { get; set; }
        public DeviceType Type { get; set; }
        public List<WebHook> Hooks { get; set; }

    }
}
