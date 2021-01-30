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
        private readonly IRecipeData recipeData;

        [BindProperty]
        public string Message { get; set; }


        [BindProperty]
        public Category Category { get; set; }

        public DeleteModel(ICategoryData categoryData, IRecipeData recipeData)
        {
            this.categoryData = categoryData;
            this.recipeData = recipeData;
        }

        public IActionResult OnGet(string categoryId)
        {
            Category = categoryData.GetById(categoryId);
            if(Category == null)
            {
                return RedirectToPage("./NotFound");
            }

            var recipeInAssignedCategory = recipeData.GetBy("CategoryId", Category.Id);

            if(recipeInAssignedCategory.Any())
            {
                Message = $"Recipe category {Category.Name} has assigned recipe(s).  Make sure you are okay unassigning category with recipe before deleting.";
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

            
            return RedirectToPage("./List", new { Message = $"{category.Name} deleted" });
        }
    }
}
