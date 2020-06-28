using ToDoList_ASPDotNETMVC_MongoStorage.WebCLI.Infrastructure;

namespace ToDoList_ASPDotNETMVC_MongoStorage.WebCLI.Commands
{
   [ConsoleCommand("help", "List available commands")]
    public class Help : IConsoleCommand
    {
        public ConsoleResult Run(string[] args)
        {
            if(args.Length > 1)
            {
                return new ConsoleErrorResult("No parameters needed");
            }
            
            string lineBreak = "<br>";
            var consoleResult = new ConsoleResult($"<div style='float:left;width:270px;'>1. 'Delete All'</div><div style='width:20px;float:left;'>-</div><div style='float:left;'>Deletes all todo items</div>{lineBreak}<div style='float:left;width:270px;'>2. 'Delete Completed'</div><div style='width:20px;float:left;'>-</div><div style='float:left;'>Deletes all todo items completed</div>{lineBreak}<div style='float:left;width:270px;'>3. Save [0]</div><div style='width:20px;float:left;'>-</div><div style='float:left;'>Save a todo item where [0] is item to save</div>"); 
            consoleResult.isHTML = true;
            
            return consoleResult;
        }
    }
}
