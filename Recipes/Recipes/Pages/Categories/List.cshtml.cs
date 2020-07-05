using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Recipes.Data;
using Recipes.Core;

namespace Recipes.Pages.Categories
{
    public class ListModel : PageModel
    {
        private readonly IConfiguration config;
        private readonly ICategoryData categoryData;

        public string Message { get; set; }
        public IEnumerable<Category> Categories { get; set; }

        public ListModel(IConfiguration config, ICategoryData categoryData)
        {
            this.config = config;
            this.categoryData = categoryData;
        }

        public void OnGet()
        {
            Message = config["Message"];
            Categories = categoryData.GetAll();
        }
    }
}
