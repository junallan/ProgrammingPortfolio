using System.Linq;
using System.Text.RegularExpressions;

namespace ToDoList_ASPDotNETMVC_MongoStorage.WebCLI.Infrastructure
{
    public class CommandLine
    {
        public string CmdLine { get; set; }

        public string[] GetArgs()
        {
            //Matches (1 or more chars that are NOT space or ") or (" any # of chars not a " followed by a ")
            var tokenEx = new Regex(@"[^\s""]+|""[^""]*""");

            return tokenEx.Matches(CmdLine)
                                 .Cast<Match>()
                                 .Select(m => m.Value.Replace("\"", ""))  //Remove " from the arg
                                 .ToArray();
            //return null;
        }
    }
}