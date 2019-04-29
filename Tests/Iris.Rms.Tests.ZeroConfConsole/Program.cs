using System;
using System.Linq;
using System.Threading.Tasks;
using Zeroconf;

namespace Iris.Rms.Tests.ZeroConfConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var resolveTask =  ResolveDomains();
            Task.WaitAll(resolveTask);
            Console.WriteLine("Finished. Hit any key to exit.");
            Console.ReadKey();
        }

        private static async Task ResolveDomains()
        {
            var domains = await ZeroconfResolver.BrowseDomainsAsync();

            var responses = await ZeroconfResolver.ResolveAsync(new string[] { "esp8266.local." },scanTime: TimeSpan.FromSeconds(20));
            // var responses = await ZeroconfResolver.ResolveAsync("_http._tcp.local.");
            //ZeroconfResolver.
            foreach (var resp in responses)
                Console.WriteLine(resp.ToString());
        }
    }
}
