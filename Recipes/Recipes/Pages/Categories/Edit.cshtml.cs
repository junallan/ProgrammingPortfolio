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

        [BindProperty]
        public Category Category { get; set; }

        public EditModel(ICategoryData categoryData)
        {
            this.categoryData = categoryData;
        }

        public IActionResult OnGet(string categoryId)
        {
            Category = string.IsNullOrEmpty(categoryId) ? new Category() : categoryData.GetById(categoryId);
            
            return Page();
        }

        public IActionResult OnPost()
        {
            Category = Category.Id == null ? categoryData.Add(Category) : categoryData.Update(Category);

            return Page();
        }


    }
}