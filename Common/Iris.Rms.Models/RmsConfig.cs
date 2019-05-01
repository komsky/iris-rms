using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Iris.Rms.Models
{
    public class RmsConfig
    {
        public RmsConfig()
        {
            Lights = new List<Light>();
            Hvacs = new List<Hvac>();
        }
        [Key]
        public int RmsId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string IpAddress { get; set; }
        public virtual ICollection<Light> Lights { get; set; }
        public virtual ICollection<Hvac> Hvacs { get; set; }
        public string MacAddress { get; set; }
    }
}
