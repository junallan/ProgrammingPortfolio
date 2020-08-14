using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDbCRUD
{
    public class MongoDatabase
    {
                private IMongoDatabase db;

        public MongoDatabase(string databaseName)
        {
            var client = new MongoClient();
            db = client.GetDatabase(databaseName);
        }

        public bool InsertRecord<T>(string tableName, T record)
        {
            try
            {
                var collection = db.GetCollection<T>(tableName);
                collection.InsertOne(record);

                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);

                return false;
            }
        }

        public List<T> LoadRecords<T>(string tableName)
        {
            var collection = db.GetCollection<T>(tableName);

            return collection.Find(new BsonDocument()).ToList();
        }

        public T LoadRecordById<T>(string table, Guid id)
        {
            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("Id", id);

            return collection.Find(filter).First();
        }

        public T LoadRecordById<T>(string table, string id)
        {
            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("Id", id);
          
            return collection.Find(filter).First();
        }

        [Obsolete]
        public bool UpsertRecord<T>(string table, string id, T record)
        {
            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("Id", id);
            //var update = Builders<T>.Update.(record)
            //collection.UpdateOne(filter, record);
            ////var update = Builders<T>.Update.Set()


            ReplaceOneResult replaceOneResult = collection.ReplaceOne(
                                filter,
                                record,
                                new UpdateOptions { IsUpsert = true }
                        ); ;
            return replaceOneResult.ModifiedCount == 1;
        }

        [Obsolete]
        public bool UpsertRecord<T>(string table, Guid id, T record)
        {
            var collection = db.GetCollection<T>(table);
            ReplaceOneResult replaceOneResult = collection.ReplaceOne(
                                new BsonDocument("_id",
                                                 id),
                                record,
                                new UpdateOptions { IsUpsert = true }
                        );
            return replaceOneResult.ModifiedCount == 1;
        }

        public bool DeleteRecord<T>(string table, Guid id)
        {
            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("Id", id);
            var result = collection.DeleteOne(filter);

            return result.DeletedCount == 1;          
        }

        //public void DeleteAllRecordsExcept(string table, List<Guid> ids)
        public bool DeleteAllRecords<T>(string table)        
        {
            var collection = db.GetCollection<T>(table);
            var result = collection.DeleteMany(new BsonDocument());        
        
            return result.DeletedCount > 0;
        }

    }
}
