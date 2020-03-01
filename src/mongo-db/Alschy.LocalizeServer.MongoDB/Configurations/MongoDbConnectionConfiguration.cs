using MongoDB.Driver;

namespace Alschy.LocalizeServer.MongoDB.Configurations
{
    public class MongoDbConnectionConfiguration
    {
        public MongoClientSettings MongoClientSettings { get; set; } = null!;

        public string DbName { get; set; } = null!;

        public string CollectionName { get; set; } = null!;
    }
}
