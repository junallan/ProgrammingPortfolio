using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Bson;
using Recipes.Core;
using Recipes.Data;

namespace Recipes.Pages.Categories
{
    public class EditModel : PageModel
    {
        private readonly ICategoryData categoryData;
        public Category Category { get; set; }

        public EditModel(ICategoryData categoryData)
        {
            this.categoryData = categoryData;
        }

        public IActionResult OnGet(string categoryId)
        {
            Category = categoryData.GetById(categoryId);
            return Page();

        }
    }
}