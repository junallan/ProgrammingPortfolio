using MongoDB.Bson;
using MongoDbCRUD;
using Recipes.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Recipes.Data
{
    public class BaseData<T> where T : IBaseItem
    {
        public readonly MongoDatabase _db;
        private string _collectionName { get; }

        public BaseData(MongoDatabase db, string collectionName)
        {
            _db = db;
            _collectionName = collectionName;
        }

        public T Add(T newItem)
        {
            newItem.Id = ObjectId.GenerateNewId().ToString();
            var newItemAdded = _db.UpsertRecord<T>(_collectionName, newItem.Id, newItem);
            return newItemAdded;
        }

        public T Delete(string id)
        {
            var item = GetById(id);
            var isDeletedItem = _db.DeleteRecord<T>(_collectionName, id);
            return isDeletedItem ? item : default;
        }

        public List<T> GetBy(string fieldName, string value)
        {
            var recs = _db.LoadRecordsBy<T>(_collectionName, fieldName, value);
            return recs;
        }

        public T GetById(string Id)
        {
            var rec = _db.LoadRecordById<T>(_collectionName, Id);
            return rec;
        }       
    }
}
