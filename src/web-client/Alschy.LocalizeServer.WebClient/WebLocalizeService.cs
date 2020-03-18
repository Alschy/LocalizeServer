using Alschy.LocalizeServer.Common.Models;
using Alschy.LocalizeServer.Common.Services;
using Alschy.LocalizeServer.Utils.Caching.Services;
using Alschy.LocalizeServer.WebClient.Configurations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Alschy.LocalizeServer.WebClient
{
    public class WebLocalizeService : ILocalizeService
    {
        private readonly ILocalizeCacheProvider? cacheProvider;
        private readonly HttpClient httpClient;
        private readonly string apiPath;

        public WebLocalizeService(WebLocalizeConfiguration configuration, ILocalizeCacheProvider? cacheProvider = null)
        {
            this.cacheProvider = cacheProvider;
            apiPath = configuration.ApiPath;
            httpClient = BuildHttpClient(configuration);
        }

        private HttpClient BuildHttpClient(WebLocalizeConfiguration configuration)
        {
            var uriBuilder = new UriBuilder()
            {
                Host = configuration.Host,
                Port = configuration.Port,
                Scheme = configuration.UseTls ? "https" : "http"
            };
            var client = new HttpClient()
            {
                BaseAddress = uriBuilder.Uri,
                Timeout = TimeSpan.FromSeconds(20)
            };
            return client;
            
        }

        public async Task<ResourceResponseModel> LocalizeAsync(ResourceRequestModel request, CancellationToken cancel)
        {
            if (cacheProvider != null)
            {
                var result = await cacheProvider.GetCachedItemAsync(request, cancel);
                if (result != null)
                {
                    return result;
                }
            }
            var sb = new StringBuilder();
            sb.Append(apiPath);
            sb.Append(request.Key);
            sb.Append("/");
            sb.Append(request.Culture);
            if (request.System != null)
            {
                sb.Append("?system=" + request.System);
            }
            var webRequest = await httpClient.GetAsync(sb.ToString());
            if (webRequest.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var response = JsonConvert.DeserializeObject<ResourceResponseModel>(await webRequest.Content.ReadAsStringAsync());
                if (response != null)
                {
                    if (cacheProvider != null && response.ResolveState.HasFlag(Common.Models.Utils.EResolveState.CultureComplete))
                    {
                        await cacheProvider.WriteChacheItemAsync(request, response, cancel);
                    }
                    return response;
                }
            }
            throw new ApplicationException("Resource Server isn't reachable");
        }
    }
}
