using Iris.Rms.Interfaces;
using Iris.Rms.Models;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Iris.Rms.Service
{
    public class RmsService : IRmsService
    {
        public async Task<HttpResponseMessage> ActOnWebHook(WebHook webHook)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                ConfigureHeaders(httpClient);
                HttpResponseMessage result;
                Console.WriteLine(webHook.Body);
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


        private void ConfigureHeaders(HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Add("X-API-Key", "Dssd2NfASDr34DTDAa9HlP8MPURVGN5bI0edwedsuay5sada");
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
