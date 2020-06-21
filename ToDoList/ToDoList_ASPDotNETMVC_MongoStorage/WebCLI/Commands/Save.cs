using ToDoList_ASPDotNETMVC_MongoStorage.Models;
using ToDoList_ASPDotNETMVC_MongoStorage.WebCLI.Infrastructure;
using ToDoList_ASPDotNETMVC_MongoStorage.ToDoListService;

namespace ToDoList_ASPDotNETMVC_MongoStorage.WebCLI.Commands
{
   [ConsoleCommand("Save", "Save a todo item")]
    public class Save : IConsoleCommand
    {
        public ConsoleResult Run(string[] args)
        {
            if(args.Length == 2)
            {    
                ToDoListService.ToDoListService toDoListService = new ToDoListService.ToDoListService();

                var result = toDoListService.SaveToDoItems(new ToDoItemsModel{ WorkItemDescription=args[1] });
 
                if(result)
                {
                    return new ConsoleResult($"To do list item {args[1]} saved successfully.");
                }
                else
                {
                    return new ConsoleResult($"Error in saving to do list item {args[1]}. No item saved.");
                }
            }
            return new ConsoleErrorResult("Command requires 1 parameter.  Enter parameter.");
        }
    }
}