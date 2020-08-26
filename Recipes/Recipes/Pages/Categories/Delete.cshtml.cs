using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Recipes.Core;
using Recipes.Data;

namespace Recipes.Pages.Categories
{
    public class DeleteModel : PageModel
    {
        private readonly ICategoryData categoryData;

        [BindProperty]
        public Category Category { get; set; }

        public DeleteModel(ICategoryData categoryData)
        {
            this.categoryData = categoryData;
        }

        public IActionResult OnGet(string categoryId)
        {
            Category = categoryData.GetById(categoryId);
            if(Category == null)
            {
                return RedirectToPage("./NotFound");
            }
            return Page();
        }

        public IActionResult OnPost(string categoryId)
        {
            var category = categoryData.Delete(categoryId);

            if(category == null)
            {
                return RedirectToPage("./NotFound");
            }

            TempData["Message"] = $"{category.Name} deleted";

            return RedirectToPage("./List");
        }
    }
}
