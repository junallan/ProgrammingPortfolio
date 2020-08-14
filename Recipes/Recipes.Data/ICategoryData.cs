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
        Category GetById(string Id);
        Category Update(Category updatedCategory);
    }


    public class CategoriesModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
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
            return recs.Select(x => new Category { Id=x.Id, Name = x.Name });
        }

        public Category GetById(string Id)
        {
            var rec = _db.LoadRecordById<Category>("Categories", Id);
            return rec;
        }

        public Category Update(Category updatedCategory)
        {
            var rec = GetById(updatedCategory.Id);
            rec.Name = updatedCategory.Name;

            _db.UpsertRecord<Category>("Categories", rec.Id, rec);

            rec = GetById(updatedCategory.Id);
            return rec;
        }
    }
    
    public class InMemoryCategoryData : ICategoryData
    {
        List<Category> categories;

        public InMemoryCategoryData()
        {
            categories = new List<Category>()
            {
                new Category{ Id=ObjectId.GenerateNewId().ToString(), Name="Breakfasts" },
                new Category{ Id=ObjectId.GenerateNewId().ToString(), Name="Meat Recipes"}
            };
        }

        public IEnumerable<Category> GetAll()
        {
            return categories;
        }

        public Category GetById(string Id)
        {
            return categories.Where(x => x.Id == Id).SingleOrDefault();
        }

        public Category Update(Category updatedCategory)
        {
            throw new NotImplementedException();
        }
    }
}
