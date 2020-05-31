namespace ToDoList_ASPDotNETMVC_MongoStorage.WebCLI.Infrastructure
{ 
  public interface IConsoleCommand
    {
        ConsoleResult Run(string[] args);
    }
}