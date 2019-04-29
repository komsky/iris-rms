using Iris.Rms.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace Iris.Rms.Interfaces
{
    public interface IRmsService
    {
        Task<HttpResponseMessage> ActOnWebHook(WebHook webHook);
    }
}
