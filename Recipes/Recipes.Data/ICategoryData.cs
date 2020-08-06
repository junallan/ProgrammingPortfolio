using Recipes.Core;
using System;
using System.Collections.Generic;
using System.Threading;
using MongoDbCRUD;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Linq;

namespace Recipes.Data
{
    public interface ICategoryData
    {
        IEnumerable<Category> GetAll();
    }


    public class CategoriesModel
    {
        [BsonId]
        public ObjectId _id { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class MongoCategoryData : ICategoryData
    {
        private MongoDatabase _db;
        public MongoCategoryData()
        {
            _db = new MongoDatabase("Recipes");

        }

        public IEnumerable<Category> GetAll()
        {
            var recs = _db.LoadRecords<CategoriesModel>("Categories");
            return recs.Select(x => new Category { Id = x.Id, Name = x.Name });
        }
    }

    public class InMemoryCategoryData : ICategoryData
    {
        List<Category> categories;

        public InMemoryCategoryData()
        {
            categories = new List<Category>()
            {
                new Category{Id=1, Name="Breakfasts"},
                new Category{Id=2, Name="Meat Recipes"}
            };
        }

        public IEnumerable<Category> GetAll()
        {
            var test = new MongoCategoryData().GetAll();
            return categories;
        }
    }
}
