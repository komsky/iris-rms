using System;

namespace Iris.Rms.Web.Host.Models
{
    public class ErrorViewModel
    {
        public bool ShowRequestId { get; set; }
        public string RequestId { get; set; }
        public string ErrorMessage { get; set; }
        public Exception Error { get; set; }
    }
}
