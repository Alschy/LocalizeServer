using Alschy.LocalizeServer.Common.Models;
using Alschy.LocalizeServer.Common.Models.Utils;
using Alschy.LocalizeServer.Common.Services;
using Alschy.LocalizeServer.MongoDB.Configurations;
using Alschy.LocalizeServer.MongoDB.StorageModels;
using Alschy.LocalizeServer.Utils.Caching.Services;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Alschy.LocalizeServer.MongoDB.Services
{
    public class MongoDbLocalizeService : ILocalizeService
    {
        private readonly IMongoCollection<MongoDbResourceHead> collection;
        private readonly ILocalizeCacheProvider? cacheProvider;

        public MongoDbLocalizeService(MongoDbConnectionConfiguration configuration, ILocalizeCacheProvider? cacheProvider = null)
        {
            var client = new MongoClient(configuration.MongoClientSettings);
            var db = client.GetDatabase(configuration.DbName);
            collection = db.GetCollection<MongoDbResourceHead>(configuration.CollectionName);
            this.cacheProvider = cacheProvider;
        }

        public async Task<ResourceResponseModel> LocalizeAsync(ResourceRequestModel request, CancellationToken cancel)
        {
            if (cacheProvider != null)
            {
                var entry = await cacheProvider.GetCachedItemAsync(request, cancel);
                if (entry != null)
                {
                    return entry;
                }
            }

            var itemCursor = await collection.FindAsync(m => m.ResourceKey == request.Key, cancellationToken: cancel);
            var item = itemCursor.FirstOrDefault();
            if (item == null)
            {
                return new ResourceResponseModel(request.Key, EResolveState.None);
            }
            var partCulture = string.Empty;
            var itemCandidates = item.ResourceItems.Where(m => m.Culture != null && m.Culture!.Equals(request.Culture, StringComparison.CurrentCultureIgnoreCase));
            IEnumerable<MongoDbResourceItem> partCultureCandidatestes = new List<MongoDbResourceItem>();
            if (request.Culture.Contains('-'))
            {
                partCulture = request.Culture.Split('-')[0];
                partCultureCandidatestes = item.ResourceItems.Where(m => m.Culture != null && m.Culture!.Equals(partCulture, StringComparison.CurrentCultureIgnoreCase));
            }
            cancel.ThrowIfCancellationRequested();
            MongoDbResourceItem? resultItem;
            if (request.System != null)
            {
                resultItem = itemCandidates.FirstOrDefault(m => m.Application != null && m.Application!.Equals(request.System, StringComparison.CurrentCultureIgnoreCase));
                if (resultItem != null)
                {
                    var result = new ResourceResponseModel(resultItem.Value, EResolveState.System|EResolveState.CultureComplete);
                    await WriteCacheItem(request, result, cancel);
                    return result;
                }
                resultItem = partCultureCandidatestes.FirstOrDefault(m => m.Application != null && m.Application!.Equals(request.System, StringComparison.CurrentCultureIgnoreCase));
                if (resultItem != null)
                {
                    return new ResourceResponseModel(resultItem.Value, EResolveState.System|EResolveState.CultureParent);
                }
            }
            cancel.ThrowIfCancellationRequested();
            resultItem = itemCandidates.FirstOrDefault(m => string.IsNullOrEmpty(m.Application));
            if (resultItem != null)
            {
                var result = new ResourceResponseModel(resultItem.Value, EResolveState.CultureComplete);
                if (request.System == null)
                {
                    await WriteCacheItem(request, result, cancel);
                }
                return result;
            }
            cancel.ThrowIfCancellationRequested();
            resultItem = partCultureCandidatestes.FirstOrDefault(m => string.IsNullOrEmpty(m.Application));
            if (resultItem != null)
            {
                return new ResourceResponseModel(resultItem.Value, EResolveState.CultureParent);
            }
            return new ResourceResponseModel(request.Key, EResolveState.None);
        }

        private async Task WriteCacheItem(ResourceRequestModel requestModel, ResourceResponseModel responseModel, CancellationToken cancel)
        {
            if (cacheProvider != null)
            {
                await cacheProvider.WriteChacheItemAsync(requestModel, responseModel, cancel);
            }
        }

    }
}
