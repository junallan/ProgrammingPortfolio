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
        public string FormTitle { get; set; }
        public enum Action
        {
            Adding,
            Editing
        }

        public EditModel(ICategoryData categoryData)
        {
            this.categoryData = categoryData;
        }

        public IActionResult OnGet(string categoryId)
        {
            if (string.IsNullOrEmpty(categoryId))
            {
                Category = new Category();
                FormTitle = $" {Action.Adding.ToString()} Category";
            }
            else
            {
                Category = categoryData.GetById(categoryId);
                FormTitle = $"{Action.Editing.ToString()} {Category.Name}";
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if(string.IsNullOrEmpty(Category.Id))
            {
                Category = categoryData.Add(Category);
                FormTitle = $"{Action.Adding.ToString()} {Category.Name}";
            }
            else
            {
                Category = categoryData.Update(Category);
                FormTitle = $"{Action.Editing.ToString()} {Category.Name}";
            }

            return Page();
        }


    }
}