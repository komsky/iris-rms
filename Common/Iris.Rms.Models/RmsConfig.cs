using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iris.Rms.Models
{
    public class RmsConfig
    {
        [Key]
        public int RmsId { get; set; }
        public string Description { get; set; }
        public virtual ICollection<RmsDevice> Devices { get; set; }
    }
}
