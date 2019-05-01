using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Iris.Rms.Models
{
    public class Hvac : RmsGenericDevice
    {
        public Hvac()
        {
            Hooks = new List<WebHook>();
        }
        [Key]
        public int RmsDeviceId { get; set; }
        public string Location { get; set; }
        public double CurrentTemperature { get; set; }
        public double SetpointTemperature { get; set; }
        public virtual ICollection<WebHook> Hooks { get; set; }

    }
}
