using ToDoList_ASPDotNETMVC_MongoStorage.WebCLI.Infrastructure;
using ToDoList_ASPDotNETMVC_MongoStorage.ToDoListService;

namespace ToDoList_ASPDotNETMVC_MongoStorage.WebCLI.Commands
{
   [ConsoleCommand("delete completed", "Deletes completed todo items")]
    public class DeleteCompleted : IConsoleCommand
    {
        public ConsoleResult Run(string[] args)
        {
            if(args.Length == 1)
            {    
                ToDoListService.ToDoListService toDoListService = new ToDoListService.ToDoListService();

                var result = toDoListService.DeleteCompletedToDoItems();
 
                if(result)
                {
                    return new ConsoleResult("To do list items completed deleted successfully.");
                }
                else
                {
                    return new ConsoleResult("Error in deleting to do list completed items. No items deleted.");
                }
            }
            return new ConsoleErrorResult("Command doesn't require parameters.  Enter without parameters.");
        }
    }
}