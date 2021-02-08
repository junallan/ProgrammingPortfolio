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
    public interface ICategoryData : IBaseData<Category>
    {
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
        private readonly MongoDatabase _db;


        public MongoCategoryData()
        {
            _db = new MongoDatabase(CollectionMappings.RecipeCollectionName);
        }

        public IEnumerable<Category> GetAll()
        {
            var recs = _db.LoadRecords<CategoriesModel>(CollectionMappings.CategoryCollectionName);
            return recs.Select(x => new Category { Id=x.Id, Name = x.Name });
        }

        public Category GetById(string Id)
        {
            var rec = _db.LoadRecordById<Category>(CollectionMappings.CategoryCollectionName, Id);
            return rec;
        }

        public List<Category> GetBy(string fieldName, string value)
        {
            var recs = _db.LoadRecordsBy<Category>(CollectionMappings.CategoryCollectionName, fieldName, value);
            return recs.ToList(); 
        }

        public Category Update(Category updatedCategory)
        {
            var rec = GetById(updatedCategory.Id);
            rec.Name = updatedCategory.Name;

            _db.UpsertRecord<Category>(CollectionMappings.CategoryCollectionName, rec.Id, rec);

            rec = GetById(updatedCategory.Id);
            return rec; 
        }

        public Category Add(Category newCategory)
        {
            newCategory.Id = ObjectId.GenerateNewId().ToString();
            var categoryAdded = _db.UpsertRecord<Category>(CollectionMappings.CategoryCollectionName, newCategory.Id, newCategory);
            return categoryAdded;
        }

        public Category Delete(string id)
        {
            var category = GetById(id);
            var isDeletedCategory =_db.DeleteRecord<Category>(CollectionMappings.CategoryCollectionName, id);
            return isDeletedCategory ? category : null;
        }
    }
    
    public class InMemoryCategoryData : ICategoryData
    {
        readonly List<Category> categories;

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
            var categoryLookedUp = categories.Where(c => c.Id == updatedCategory.Id).SingleOrDefault();
            
            if(categoryLookedUp != null)
            {
                categoryLookedUp.Name = updatedCategory.Name;
            }

            return categoryLookedUp;
        }

        public Category Add(Category newCategory)
        {
            categories.Add(newCategory);

            return newCategory;
        }

        public Category Delete(string Id)
        {
            var categoryToDelete = GetById(Id);
            categories.Remove(categoryToDelete);

            return categoryToDelete;
        }

        public List<Category> GetBy(string fieldName, string value)
        {
            throw new NotImplementedException();
        }
    }
}
