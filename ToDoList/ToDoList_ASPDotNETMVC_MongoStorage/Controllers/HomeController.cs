using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using ToDoList_ASPDotNETMVC_MongoStorage.Models;

namespace ToDoList_ASPDotNETMVC_MongoStorage.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
       public JsonResult GetToDoItems() 
        {
            MongoCRUD db = new MongoCRUD("ToDo");
            var recs = db.LoadRecords<ToDoItemsModel>("ToDoItems");

            // foreach(var rec in recs)
            // {
            //     Console.WriteLine($"{rec.Id}: {rec.State}, {rec.WorkItemDescription}");
            // }

            return Json(recs);
        }

        [HttpPost]
        public JsonResult SaveToDoItems(List<ToDoItemsModel> items)
        {


            return Json(true);
        }
        public IActionResult Index()
        {
                //MongoCRUD db = new MongoCRUD("ToDo");

               // ToDoItemsModel model = new ToDoItemsModel{ Id= Guid.NewGuid(), WorkItemDescription="Learning Functional Programming", State=2};

            //db.UpsertRecord<ToDoItemsModel>("ToDoItems",model.Id,model);

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public class ToDoItemsModel
    {
        [BsonId]
        public Guid Id{get;set;}
        public string WorkItemDescription{get;set;}
        public int State{get;set;}
    }

    public class MongoCRUD
    {
        private IMongoDatabase db;

        public MongoCRUD(string databaseName)
        {
            var client = new MongoClient();
            db = client.GetDatabase(databaseName);
        }

        public void InsertRecord<T>(string tableName, T record)
        {
            var collection = db.GetCollection<T>(tableName);
            collection.InsertOne(record);
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

        [Obsolete]
        public void UpsertRecord<T>(string table, Guid id, T record)
        {
            var collection = db.GetCollection<T>(table);

            ReplaceOneResult replaceOneResult = collection.ReplaceOne(
                                new BsonDocument("_id",
                                                 id),
                                record,
                                new UpdateOptions { IsUpsert = true }
                        );
            var result = replaceOneResult;
        }

        public void DeleteRecord<T>(string table, Guid id)
        {
            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("Id", id);
            collection.DeleteOne(filter);
        }
}
}
