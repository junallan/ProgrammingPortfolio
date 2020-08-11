using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Recipes.Core
{
    public class Category
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
