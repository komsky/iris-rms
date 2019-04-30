using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Iris.Rms.Models.Suite
{
    public class Zone
    {
        [Key]
        public int ZoneId { get; set; }
        public string LocationName { get; set; }
        public virtual ICollection<Hvac> Hvacs { get; set; }
        public virtual ICollection<Shade> Shades { get; set; }
        public virtual ICollection<Light> Lights { get; set; }
        public virtual ICollection<TV> Tvs { get; set; }
    }
}
