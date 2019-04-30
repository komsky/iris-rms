using System.ComponentModel.DataAnnotations;

namespace Iris.Rms.Models.Suite
{
    public class Shade : ObjectData
    {
        [Key]
        public int ShadeId { get; set; }
        public string Name { get; set; }
        public ShadeType Type { get; set; }

    }
}
