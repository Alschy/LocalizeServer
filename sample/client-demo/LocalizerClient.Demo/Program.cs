using Alschy.LocalizeServer.Common.Models;
using Alschy.LocalizeServer.Common.Services;
using Alschy.LocalizeServer.Utils.Caching.Memory.Services;
using Alschy.LocalizeServer.Utils.Caching.Services;
using Alschy.LocalizeServer.WebClient;
using Alschy.LocalizeServer.WebClient.Configurations;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace LocalizerClient.Demo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var provider = new ServiceCollection()
                .AddMemoryCache()
                .AddSingleton(new WebLocalizeConfiguration
                {
                    Host = "localhost",
                    Port = 5001
                })
                .AddTransient<ILocalizeCacheProvider, MemoryLocalizeCacheProvider>()
                .AddTransient<ILocalizeService, WebLocalizeService>()
                .BuildServiceProvider();

            var serivce = provider.GetRequiredService<ILocalizeService>();

            var request = new ResourceRequestModel("test", "de-de", null);

            var stopwatch = new Stopwatch();
            Console.WriteLine("Start request 1...");
            stopwatch.Start();
            var response = await serivce.LocalizeAsync(request, CancellationToken.None);
            stopwatch.Stop();
            Console.WriteLine($"Ergebnis: {response.Value} in {stopwatch.ElapsedMilliseconds}ms.");

            stopwatch = new Stopwatch();
            Console.WriteLine("Start request 2...");
            stopwatch.Start();
            response = await serivce.LocalizeAsync(request, CancellationToken.None);
            stopwatch.Stop();
            Console.WriteLine($"Ergebnis: {response.Value} in {stopwatch.ElapsedMilliseconds}ms.");
            Console.ReadKey();
        }
    }
}
