using ToDoList_ASPDotNETMVC_MongoStorage.WebCLI.Infrastructure;
using ToDoList_ASPDotNETMVC_MongoStorage.ToDoListService;

namespace ToDoList_ASPDotNETMVC_MongoStorage.WebCLI.Commands
{
   [ConsoleCommand("delete all", "Deletes all todo items")]
    public class DeleteAll : IConsoleCommand
    {
        public ConsoleResult Run(string[] args)
        {
            if(args.Length == 1)
            {    
                ToDoListService.ToDoListService toDoListService = new ToDoListService.ToDoListService();

                var result = toDoListService.DeleteAllToDoItems();
 
                if(result)
                {
                    return new ConsoleResult("To do list items deleted successfully.");
                }
                else
                {
                    return new ConsoleResult("Error in deleting to do list items. No items deleted.");
                }
            }
            return new ConsoleErrorResult("Command doesn't require parameters.  Enter without parameters.");
        }
    }
}