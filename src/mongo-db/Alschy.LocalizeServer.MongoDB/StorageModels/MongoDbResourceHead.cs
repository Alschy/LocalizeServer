﻿using Alschy.LocalizeServer.Core.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alschy.LocalizeServer.MongoDB.StorageModels
{
    class MongoDbResourceHead : ResourceHead<MongoDbResourceItem>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonRequired]
        public new string ResourceKey { get; set; } = null!;

        public override IList<MongoDbResourceItem> ResourceItems { get; set; } = new List<MongoDbResourceItem>();
    }
}
