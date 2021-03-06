﻿using System;
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
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                return false;
            }
        }

        public List<T> LoadRecordsOr<T>(string tableName, FilterDefinition<T>[] filters)
        {
            var collection = db.GetCollection<T>(tableName);
            var filter = Builders<T>.Filter.Or(filters);
            return collection.Find(filter).ToList();
        }

        public List<T> LoadRecords<T>(string tableName)
        {
            var collection = db.GetCollection<T>(tableName);

            return collection.Find(new BsonDocument()).ToList();
        }

        public List<T> LoadRecordsIn<T>(string tableName, string fieldName, List<string> values)
        {
            
            var collection = db.GetCollection<T>(tableName);
            var filter = FilterDefinitionIn<T>(fieldName, values);
            return collection.Find(filter).ToList();
        }

        public static FilterDefinition<T> FilterDefinitionIn<T>(string fieldName, List<string> values)
        {
            return Builders<T>.Filter.In(fieldName, values);
        }

        public List<T> LoadRecordsBy<T>(string tableName, string fieldName, string value)
        {
            var collection = db.GetCollection<T>(tableName);
            var filter = FilterDefinitionEqual<T>(fieldName, value);

            return collection.Find(filter).ToList();
        }

        public static FilterDefinition<T> FilterDefinitionEqual<T>(string fieldName, string value)
        {
            return Builders<T>.Filter.Eq(fieldName, value);
        }

        public List<T> LoadRecordsLike<T>(string tableName, string fieldName, string value)
        {
            var collection = db.GetCollection<T>(tableName);
            var filter = FilterDefinitionLike<T>(fieldName, value); 
            return collection.Find(filter).ToList();
        }

        public List<T> LoadRecordsLike<T>(string tableName, string fieldName, List<string> values)
        {
            var collection = db.GetCollection<T>(tableName);
            var filters = FilterDefinitionLike<T>(fieldName, values);
            return LoadRecordsOr<T>(tableName, filters.ToArray());
        }
        
        public List<TItems> LoadRecordsByRange<TItems,TValue>(string tableName, string fieldName, TValue startValue, TValue endValue)
        {
            var collection = db.GetCollection<TItems>(tableName);
            var filter = FilterDefinitionRange<TItems, TValue>(fieldName, startValue, endValue);
            return collection.Find(filter).ToList();
        }

        public static FilterDefinition<TItems> FilterDefinitionRange<TItems, TValue>(string fieldName, TValue startValue, TValue endValue)
        {
            var filterDefinition = Builders<TItems>.Filter.Gte(fieldName, startValue) & Builders<TItems>.Filter.Lte(fieldName, endValue);
            return filterDefinition;
        }

        public List<T> LoadRecordsGreaterThanOrEqual<T>(string tableName, string fieldName, string value)
        {
            var collection = db.GetCollection<T>(tableName);
            var filter = FilterDefinitionGreaterThanOrEqual<T>(fieldName, value);
            return collection.Find(filter).ToList();
        }

        public static FilterDefinition<T> FilterDefinitionGreaterThanOrEqual<T>(string fieldName, string value)
        {
            var filterDefinition = Builders<T>.Filter.Gte(fieldName, value);
            return filterDefinition;
        }

        public List<T> LoadRecordsLessThanOrEqual<T>(string tableName, string fieldName, string value)
        {
            var collection = db.GetCollection<T>(tableName);
            var filter = FilterDefinitionLessThanOrEqual<T>(fieldName, value);
            return collection.Find(filter).ToList();
        }

        public static FilterDefinition<T> FilterDefinitionLessThanOrEqual<T>(string fieldName, string value)
        {
            var filterDefinition =  Builders<T>.Filter.Lte(fieldName, value);
            return filterDefinition;
        }

        public static List<FilterDefinition<T>> FilterDefinitionLike<T>(string fieldName, List<string> values)
        {
            List<FilterDefinition<T>> resultFilters = new List<FilterDefinition<T>>();

            foreach (var value in values)
            {
                resultFilters.Add(Builders<T>.Filter.Regex(fieldName, new BsonRegularExpression($".*{value}.*")));
            }

            return resultFilters;
        }

        public static FilterDefinition<T> FilterDefinitionLike<T>(string fieldName, string value)
        {
            return Builders<T>.Filter.Regex(fieldName, new BsonRegularExpression($".*{value}.*"));
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

            return collection.Find(filter).FirstOrDefault();
        }


        public T UpsertRecord<T>(string table, string id, T record)
        {
            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("Id", id);
            ReplaceOneResult replaceOneResult = collection.ReplaceOne(
                                filter,
                                record,
                                new ReplaceOptions { IsUpsert = true }
                        ); 

            return LoadRecordById<T>(table, id);
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
            var filter = Builders<T>.Filter.Eq("Id", id);
            return DeleteRecord(table, filter);
        }

        public bool DeleteRecord<T>(string table, string id)
        {
            var filter = Builders<T>.Filter.Eq("Id", id);
            return DeleteRecord(table, filter);
        }

        private bool DeleteRecord<T>(string table, FilterDefinition<T> filter)
        {
            var collection = db.GetCollection<T>(table);
            var result = collection.DeleteOne(filter);

            return result.DeletedCount == 1;
        }

        public bool DeleteAllRecords<T>(string table)        
        {
            var collection = db.GetCollection<T>(table);
            var result = collection.DeleteMany(new BsonDocument());        
        
            return result.DeletedCount > 0;
        }

    }
}
