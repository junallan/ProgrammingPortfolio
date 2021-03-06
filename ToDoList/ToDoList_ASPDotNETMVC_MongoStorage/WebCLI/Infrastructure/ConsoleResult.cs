namespace ToDoList_ASPDotNETMVC_MongoStorage.WebCLI.Infrastructure
{
    public class ConsoleResult
    {
        public string output  { get; set; } = "";     //Holds the success or error output
        public bool   isError { get; set; } = false;  //True if output is an error message
        public bool   isHTML  { get; set; } = false;  //Is the output a text string or an HTML string?

        public ConsoleResult() { }
        public ConsoleResult(string output)
        {
            this.output = output;
        }
    }

    public class ConsoleErrorResult : ConsoleResult
    {
        public ConsoleErrorResult()
        {
            isError = true;
            output  = "Invalid syntax";
        }

        public ConsoleErrorResult(string message)
        {
            isError = true;
            output  = message;
        }
    }
}

