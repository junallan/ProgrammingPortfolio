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

              return _db.DeleteAllRecords<ToDoItemsModel>("ToDoItems");
              //TODO: It's better if DeleteAllRecords returns success or failure,
              //same in other methods 
              //return true;
        }

        public bool DeleteCompletedToDoItems()
        {
            bool result = true;

            var completedRecordsToDelete = _db.LoadRecords<ToDoItemsModel>("ToDoItems")
                                                .Where(x => x.State != 0).ToList();

            foreach(var itm in completedRecordsToDelete)
            {
                if(!_db.DeleteRecord<ToDoItemsModel>("ToDoItems", itm.Id)){ result = false; }
            }                

            //  _db.LoadRecords<ToDoItemsModel>("ToDoItems")
            //                 .Where(x => x.State != 0).ToList()
            //                 .ForEach(x => _db.DeleteRecord<ToDoItemsModel>("ToDoItems", x.Id));

            return result;
        }

        public bool SaveToDoItems(ToDoItemsModel item)
        {
            bool result;

            if(item.Id == System.Guid.Empty)
            {
               result = _db.InsertRecord<ToDoItemsModel>("ToDoItems",item);
            }
            else
            {
                result = _db.UpsertRecord<ToDoItemsModel>("ToDoItems", item.Id, item);
            }

            return result;
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