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
            string space = "&nbsp;";
            var consoleResult = new ConsoleResult($"1. 'Delete All'{space}{space}{space}{space}{space}{space}{space}- Deletes all todo items {lineBreak} 2. 'Delete Completed' - Deletes all todo items completed {lineBreak} 3. Save [0] {space}{space}{space}{space}{space}{space}{space}{space}{space}{space}- Save a todo item where [0] is item to save"); 
            consoleResult.isHTML = true;
            return consoleResult;
        }
    }
}
