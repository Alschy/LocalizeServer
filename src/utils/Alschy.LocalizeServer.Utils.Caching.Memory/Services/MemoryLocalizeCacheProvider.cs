using Alschy.LocalizeServer.Common.Models;
using Alschy.LocalizeServer.Utils.Caching.Services;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Alschy.LocalizeServer.Utils.Caching.Memory.Services
{
    public class MemoryLocalizeCacheProvider : ILocalizeCacheProvider
    {
        private readonly IMemoryCache memoryCache;

        public MemoryLocalizeCacheProvider(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public Task<ResourceResponseModel?> GetCachedItemAsync(ResourceRequestModel model, CancellationToken cancel)
        {
            var hashKey = BuildRequestHashCode(model);
            cancel.ThrowIfCancellationRequested();
            var success = memoryCache.TryGetValue(hashKey, out ResourceResponseModel result);
            if (success)
            {
                return Task.FromResult<ResourceResponseModel?>(result);
            }
            else
            {
                return Task.FromResult<ResourceResponseModel?>(null);
            }
        }

        public Task WriteChacheItemAsync(ResourceRequestModel requestModel,ResourceResponseModel responseModel , CancellationToken cancel)
        {
            var hashKey = BuildRequestHashCode(requestModel);
            cancel.ThrowIfCancellationRequested();
            memoryCache.Set(hashKey, responseModel);
            return Task.CompletedTask;
        }

        private string BuildRequestHashCode(ResourceRequestModel model)
        {
            var hashAlg = MD5.Create();
            var sb = new StringBuilder();
            sb.Append(typeof(ResourceRequestModel).FullName);
            sb.Append("!+");
            sb.Append(model.Key);
            sb.Append(model.Culture);
            sb.Append(model.System);
            var array = Encoding.ASCII.GetBytes(sb.ToString());

            var hash = hashAlg.ComputeHash(array);
            return Encoding.ASCII.GetString(hash);
        }

    }
}
