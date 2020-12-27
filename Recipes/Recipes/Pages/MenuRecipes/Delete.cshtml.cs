using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Recipes.Core;
using Recipes.Data;

namespace Recipes.Pages.MenuRecipes
{
    public class DeleteModel : PageModel
    {
        private readonly IRecipeData recipeData;
        private readonly ICategoryData categoryData;

        [BindProperty]
        public DetailModel DetailModel { get; set; }

        public DeleteModel(IRecipeData recipeData, ICategoryData categoryData)
        {
            this.recipeData = recipeData;
            this.categoryData = categoryData;
            DetailModel = new DetailModel(recipeData,categoryData);
        }

        public IActionResult OnGet(string recipeId)
        {
            DetailModel.Recipe = recipeData.GetById(recipeId);

            if (DetailModel.Recipe == null)
            {
                return RedirectToPage("./NotFound");
            }

            DetailModel.CategoryNameOfRecipe = categoryData.GetById(DetailModel.Recipe.CategoryId)?.Name;

            return Page();
        }

        public IActionResult OnGetDeleteMenuRecipe(string recipeId)
        {
            var recipe = recipeData.Delete(recipeId);

            if(recipe == null)
            {
                TempData["Message"] = $"Error deleting {recipe.Name}";
            }
            else
            {
                TempData["Message"] = $"{recipe.Name} deleted";
            }

            return new JsonResult(string.Empty);
        }
    }
}
