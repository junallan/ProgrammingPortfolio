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
        private readonly ICategoryData categoryData;

        [BindProperty(SupportsGet = true)]
        public string Message { get; set; }
        public IEnumerable<Category> Categories { get; set; }

        public ListModel(ICategoryData categoryData)
        {
            this.categoryData = categoryData;
        }

        public void OnGet()
        {           
            Categories = categoryData.GetAll();
        }
    }
}
