using System.ComponentModel.DataAnnotations;

namespace Iris.Rms.Models.Suite
{
    public class Hvac : ObjectData
    {
        [Key]
        public int HvacId { get; set; }
        public HvacMode Mode { get; set; }
        public HvacFanMode FanMode { get; set; }
        /// <summary>
        /// Celsius only, please keep Fahrenheit as calculated value
        /// </summary>
        public double CurrentTemperature { get; set; }
        public double SetpointTemperature { get; set; }
    }
}
