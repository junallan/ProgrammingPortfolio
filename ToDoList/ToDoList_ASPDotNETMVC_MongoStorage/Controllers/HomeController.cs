using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ToDoList_ASPDotNETMVC_MongoStorage.Models;
using ToDoList_ASPDotNETMVC_MongoStorage.WebCLI.Infrastructure;
using System.Reflection;
using ToDoList_ASPDotNETMVC_MongoStorage.ToDoListService;

namespace ToDoList_ASPDotNETMVC_MongoStorage.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public static ToDoListService.ToDoListService _toDoListService;
        public static readonly Type AttributeType = typeof(ConsoleCommandAttribute);
        public static readonly List<Type> CommandTypes;

        static HomeController()
        {
            _toDoListService = new ToDoListService.ToDoListService();
            
            var type = typeof(IConsoleCommand);
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(asm => asm.GetTypes());

            CommandTypes = types.Where(t => t.GetInterfaces().Contains(type)).ToList();
        }


        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public ConsoleResult WebCLI([FromBody] CommandLine command)
        {
            var args = command.GetArgs();
            var cmd  = args.First().ToUpper();
            Type cmdTypeToRun = null;

            //Get command type
            foreach (var cmdType in CommandTypes)
            {
                var attr = (ConsoleCommandAttribute)cmdType.GetTypeInfo().GetCustomAttributes(AttributeType).FirstOrDefault();
                if(attr != null && attr.Name.ToUpper() == cmd)
                {
                    cmdTypeToRun = cmdType; break;
                }
            }
            if(cmdTypeToRun == null) { return new ConsoleErrorResult(); }
            

            //Instantiate and run the command
            try
            {
                var cmdObj = Activator.CreateInstance(cmdTypeToRun) as IConsoleCommand;
                return cmdObj.Run(args);
            }
            catch
            {
                return new ConsoleErrorResult();
            }
        }

        [HttpGet]
       public JsonResult GetToDoItems() 
        {
            return Json(_toDoListService.GetToDoItems());
        }

        [HttpPost]
        public JsonResult DeleteAllToDoItems()
        {
            return Json(_toDoListService.DeleteAllToDoItems());
        }

        [HttpPost]
        public JsonResult DeleteCompletedToDoItems()
        {
            return Json(_toDoListService.DeleteCompletedToDoItems());
        }

        [HttpPost]
        public JsonResult SaveToDoItems(ToDoItemsModel item)
        {
            return Json(_toDoListService.SaveToDoItems(item));
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
