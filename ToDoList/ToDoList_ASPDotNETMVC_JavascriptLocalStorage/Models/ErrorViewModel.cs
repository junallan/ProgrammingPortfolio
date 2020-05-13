using System;

namespace ToDoList_ASPDotNETMVC_JavascriptLocalStorage.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
