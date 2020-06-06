using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ToDoList_ASPDotNETMVC_MongoStorage.Models
{
    public class ToDoItemsModel
    {
         [BsonId]
        public System.Guid Id{get;set;}
        public string WorkItemDescription{get;set;}
        public int State{get;set;}
    }
}