using Iris.Rms.Models.Enums;
using System.Collections.Generic;

namespace Iris.Rms.Models
{
    public class RmsDevice
    {
        public int RmsDeviceId { get; set; }
        public DeviceType Type { get; set; }
        public List<WebHook> Hooks { get; set; }

    }
}
