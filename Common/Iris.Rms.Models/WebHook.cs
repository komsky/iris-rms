using Iris.Rms.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Iris.Rms.Models
{
    public class WebHook
    {
        [Key]
        public int WebHookId { get; set; }
        public string HookUrl { get; set; }
        public string Method { get; set; }
        public RmsCommand ActivationCommand { get; set; }
        public string ActivationCommandDescription { get { return ActivationCommand.ToString(); } }
        public string Body { get; set; }

    }
}
