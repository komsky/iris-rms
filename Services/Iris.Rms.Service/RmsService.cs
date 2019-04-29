using Iris.Rms.Interfaces;
using Iris.Rms.Models;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Iris.Rms.Service
{
    public class RmsService : IRmsService
    {
        public async Task<HttpResponseMessage> ActOnWebHook(WebHook webHook)
        {
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage result;
                switch (webHook.Method)
                {
                    case "POST":
                        result = await httpClient.PostAsync(await ResolveMdns(webHook.HookUrl), new StringContent(webHook.Body, Encoding.UTF8, "application/json"));
                        result.EnsureSuccessStatusCode();
                        break;
                    case "GET":
                    default:
                        result = await httpClient.GetAsync(await ResolveMdns(webHook.HookUrl));
                        result.EnsureSuccessStatusCode();
                        break;
                }
                return result;
            }
        }

        private async Task<string> ResolveMdns(string hookUrl)
        {
            //TODO: resolve zero-config URL
            //var resolved = await Zeroconf.ZeroconfResolver.ResolveAsync(hookUrl);
            //var doesNotResolve = resolved.First().IPAddress;
            return hookUrl;
        }
    }
}
