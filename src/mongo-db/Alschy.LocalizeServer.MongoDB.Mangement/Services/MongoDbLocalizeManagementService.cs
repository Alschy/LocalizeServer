using Alschy.LocalizeServer.Core.Management.Models;
using Alschy.LocalizeServer.Core.Management.Services;
using Alschy.LocalizeServer.MongoDB.Configurations;
using Alschy.LocalizeServer.MongoDB.StorageModels;
using MongoDB.Driver;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Alschy.LocalizeServer.MongoDB.Mangement.Services
{
    public class MongoDbLocalizeManagementService : ILocalizeManagementService
    {
        private readonly IMongoCollection<MongoDbResourceHead> collection;

        public MongoDbLocalizeManagementService(MongoDbConnectionConfiguration configuration)
        {
            var client = new MongoClient(configuration.MongoClientSettings);
            var db = client.GetDatabase(configuration.DbName);
            collection = db.GetCollection<MongoDbResourceHead>(configuration.CollectionName);
        }

        public async Task DeleteResourceItem(ResourceRemoveRequestModel model, CancellationToken cancel)
        {
            var pointer = await collection.FindAsync(m => m.ResourceKey == model.Key);
            var head = await pointer.FirstOrDefaultAsync();
            bool isAnyToDo = false;
            if (string.IsNullOrEmpty(model.Culture))
            {
                await collection.DeleteOneAsync(m => m.ResourceKey == model.Key);
                return;
            }
            else if (string.IsNullOrEmpty(model.Application))
            {
                var items = head.ResourceItems.Where(m => m.Culture == model.Culture).ToList();
                if (items.Any())
                {
                    foreach (var item in items)
                    {
                        head.ResourceItems.Remove(item);
                    }
                    isAnyToDo = true;
                }
            }
            else
            {
                var item = head.ResourceItems.Where(m => m.Culture == model.Culture && m.Application == model.Application).FirstOrDefault();
                if (item != null)
                {
                    head.ResourceItems.Remove(item);
                    isAnyToDo = true;
                }
            }
            if (isAnyToDo)
            {
                await collection.ReplaceOneAsync(m => m.ResourceKey == model.Key, head);
            }
        }

        public async Task ModifyResourceItem(ResourceModifyRequestModel model, CancellationToken cancel)
        {
            bool addMode = false;
            var pointer = await collection.FindAsync(m => m.ResourceKey == model.Key);
            var head = await pointer.FirstOrDefaultAsync();
            if (head == null)
            {
                head = BuildHead(model.Key);
                addMode = true;
            }
            var item = head.ResourceItems.FirstOrDefault(m => m.Culture == model.Culture && m.Application == model.Application);
            if (item == null)
            {
                head.ResourceItems.Add(BuildItem(model.Culture, model.Value, model.Application));
            }
            else
            {
                item.Value = model.Value;
            }
            if (addMode)
            {
                await collection.InsertOneAsync(head);
            }
            else
            {
                await collection.ReplaceOneAsync(m => m.ResourceKey == model.Key, head);
            }
        }

        private MongoDbResourceHead BuildHead(string key)
        {
            return new MongoDbResourceHead()
            {
                ResourceKey = key
            };
        }

        private MongoDbResourceItem BuildItem(string culture, string value, string? application)
        {
            return new MongoDbResourceItem()
            {
                Application = application,
                Culture = culture.ToUpper(),
                Value = value
            };
        }
    }
}
