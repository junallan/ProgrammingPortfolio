using ToDoList_ASPDotNETMVC_MongoStorage.WebCLI.Infrastructure;

namespace ToDoList_ASPDotNETMVC_MongoStorage.WebCLI.Commands
{
   [ConsoleCommand("delete all", "Deletes all todo items")]
    public class DeleteAll : IConsoleCommand
    {
        public ConsoleResult Run(string[] args)
        {
            if(args.Length > 1)
            {

                return new ConsoleResult(args[1]);
            }
            return new ConsoleErrorResult("I didn't hear anything!");
        }
    }
}