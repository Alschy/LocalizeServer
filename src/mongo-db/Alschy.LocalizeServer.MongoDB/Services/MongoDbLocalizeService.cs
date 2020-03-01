using Alschy.LocalizeServer.Common.Models;
using Alschy.LocalizeServer.Common.Models.Utils;
using Alschy.LocalizeServer.Common.Services;
using Alschy.LocalizeServer.MongoDB.Configurations;
using Alschy.LocalizeServer.MongoDB.StorageModels;
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
        private object indexCreationLock;

        public MongoDbLocalizeService(MongoDbConnectionConfiguration configuration)
        {
            indexCreationLock = new object();
            var client = new MongoClient(configuration.MongoClientSettings);
            var db = client.GetDatabase(configuration.DbName);
            collection = db.GetCollection<MongoDbResourceHead>(configuration.CollectionName);
            CreateIndex();
        }

        public async Task<ResourceResponseModel> LocalizeAsync(ResourceRequestModel request, CancellationToken cancel)
        {
            lock (indexCreationLock)
            { }
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
                    return new ResourceResponseModel(resultItem.Value, EResolveState.System|EResolveState.CultureComplete);
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
                return new ResourceResponseModel(resultItem.Value, EResolveState.CultureComplete);
            }
            cancel.ThrowIfCancellationRequested();
            resultItem = partCultureCandidatestes.FirstOrDefault(m => string.IsNullOrEmpty(m.Application));
            if (resultItem != null)
            {
                return new ResourceResponseModel(resultItem.Value, EResolveState.CultureParent);
            }
            return new ResourceResponseModel(request.Key, EResolveState.None);
        }

        private void CreateIndex()
        {
            lock (indexCreationLock)
            {
                collection.Indexes.CreateOneAsync(new CreateIndexModel<MongoDbResourceHead>(Builders<MongoDbResourceHead>.IndexKeys.Text(m => m.ResourceKey))).Wait();
            }
        }

    }
}
