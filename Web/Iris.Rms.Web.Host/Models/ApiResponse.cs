using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Iris.Rms.Web.Host.Models
{
    public class ApiResponse
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public Status Status { get; set; }
        public string Message { get; set; }
    }

    public enum Status
    {
        Error, Success
    }
}
