using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using ToDoList_ASPDotNETMVC_MongoStorage.Models;
using MongoDbCRUD;

namespace ToDoList_ASPDotNETMVC_MongoStorage.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private MongoDatabase  _db;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _db = new MongoDatabase("ToDo");
        }

        [HttpGet]
       public JsonResult GetToDoItems() 
        {
            var recs = _db.LoadRecords<ToDoItemsModel>("ToDoItems");

            // foreach(var rec in recs)
            // {
            //     Console.WriteLine($"{rec.Id}: {rec.State}, {rec.WorkItemDescription}");
            // }

            return Json(recs);
        }

        [HttpPost]
        public JsonResult DeleteAllToDoItems()
        {
            _db.DeleteAllRecords<ToDoItemsModel>("ToDoItems");
            //db.LoadRecords<ToDoItemsModel>("ToDoItems").ToList()
            //        .ForEach(x => db.DeleteRecord<ToDoItemsModel>("ToDoItems", x.Id));

            return Json(true);
        }

        [HttpPost]
        public JsonResult DeleteCompletedToDoItems()
        {
            _db.LoadRecords<ToDoItemsModel>("ToDoItems")
                    .Where(x => x.State != 0).ToList()
                    .ForEach(x => _db.DeleteRecord<ToDoItemsModel>("ToDoItems", x.Id));


            return Json(true);
        }

        [HttpPost]
        public JsonResult SaveToDoItems(ToDoItemsModel item)
        {
            if(item.Id == Guid.Empty)
            {
                _db.InsertRecord<ToDoItemsModel>("ToDoItems",item);
            }
            else
            {
                _db.UpsertRecord<ToDoItemsModel>("ToDoItems", item.Id, item);
            }

            return Json(true);
        }
        public IActionResult Index()
        {
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
}
