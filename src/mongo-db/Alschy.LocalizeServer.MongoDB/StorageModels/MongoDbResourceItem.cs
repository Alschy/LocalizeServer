using Alschy.LocalizeServer.Core.Models;

namespace Alschy.LocalizeServer.MongoDB.StorageModels
{
    class MongoDbResourceItem : ResourceItem
    {
        public new string? Application { get; set; }

        public new string? Culture { get; set; }

        public new string Value { get; set; } = null!;
    }
}
