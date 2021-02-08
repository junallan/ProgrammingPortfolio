using MongoDbCRUD;
using System;
using System.Collections.Generic;
using System.Text;

namespace Recipes.Data
{
    public class BaseData<T>
    {
        public readonly MongoDatabase _db;
        private string _collectionName { get; }

        public BaseData(MongoDatabase db, string collectionName)
        {
            _db = db;
            _collectionName = collectionName;
        }

        public List<T> GetBy(string fieldName, string value)
        {
            var recs = _db.LoadRecordsBy<T>(_collectionName, fieldName, value);
            return recs;
        }
    }
}
