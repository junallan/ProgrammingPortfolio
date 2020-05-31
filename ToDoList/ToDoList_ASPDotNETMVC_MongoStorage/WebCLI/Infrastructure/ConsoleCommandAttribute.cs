

namespace ToDoList_ASPDotNETMVC_MongoStorage.WebCLI.Infrastructure
{ 
   [System.AttributeUsage(System.AttributeTargets.Class)]
    public class ConsoleCommandAttribute : System.Attribute
    {
        public string Name        { get; set; }
        public string Description { get; set; }

        public ConsoleCommandAttribute(string name, string description)
        {
            Name        = name;
            Description = description;
        }
    }
}

