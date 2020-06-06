using ToDoList_ASPDotNETMVC_MongoStorage.Models;
using MongoDB.Driver;
using MongoDbCRUD;
using System.Linq;
using System.Collections.Generic;

namespace ToDoList_ASPDotNETMVC_MongoStorage.ToDoListService
{
    public class ToDoListService
    {
        private MongoDatabase  _db;

        public ToDoListService()
        {
            _db = new MongoDatabase("ToDo");
        }

        public bool DeleteAllToDoItems()
        {
                      //_db.DeleteAllRecords<ToDoItemsModel>("ToDoItems");
            //db.LoadRecords<ToDoItemsModel>("ToDoItems").ToList()
            //        .ForEach(x => db.DeleteRecord<ToDoItemsModel>("ToDoItems", x.Id));

              _db.DeleteAllRecords<ToDoItemsModel>("ToDoItems");
              //TODO: It's better if DeleteAllRecords returns success or failure,
              //same in other methods 
              return true;
        }

        public bool DeleteCompletedToDoItems()
        {
            _db.LoadRecords<ToDoItemsModel>("ToDoItems")
                    .Where(x => x.State != 0).ToList()
                    .ForEach(x => _db.DeleteRecord<ToDoItemsModel>("ToDoItems", x.Id));

            return true;
        }

        public bool SaveToDoItems(ToDoItemsModel item)
        {
            if(item.Id == System.Guid.Empty)
            {
                _db.InsertRecord<ToDoItemsModel>("ToDoItems",item);
            }
            else
            {
                _db.UpsertRecord<ToDoItemsModel>("ToDoItems", item.Id, item);
            }

            return true;
        }

        public List<ToDoItemsModel> GetToDoItems() 
        {
            var recs = _db.LoadRecords<ToDoItemsModel>("ToDoItems");

            // foreach(var rec in recs)
            // {
            //     Console.WriteLine($"{rec.Id}: {rec.State}, {rec.WorkItemDescription}");
            // }

            return recs;
        }
    }
}