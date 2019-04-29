using Iris.Rms.Interfaces;
using Iris.Rms.Models;
using System;
using System.Collections.Generic;
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
                        result = await httpClient.PostAsync(webHook.HookUrl, new StringContent(webHook.Body, Encoding.UTF8, "application/json"));
                        result.EnsureSuccessStatusCode();
                        break;
                    case "GET":
                    default:
                        result = await httpClient.GetAsync(webHook.HookUrl);
                        result.EnsureSuccessStatusCode();
                        break;
                }
                return result;
            }
        }
    }
}
