using System.ComponentModel.DataAnnotations;

namespace Iris.Rms.Models
{
    public class WebHook
    {
        [Key]
        public int WebHookHookId { get; set; }
        public string HookUrl { get; set; }
        public string Method { get; set; }
        public string Body { get; set; }

        public int RmsId { get; set; }
        //add some kind of trigger
    }
}
