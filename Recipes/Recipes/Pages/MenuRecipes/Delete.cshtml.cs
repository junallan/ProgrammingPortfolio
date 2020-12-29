using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Recipes.Core;
using Recipes.Data;
using Recipes.ViewComponents;

namespace Recipes.Pages.MenuRecipes
{
    public class DeleteModel : PageModel
    {
        private readonly IRecipeData recipeData;
        private readonly ICategoryData categoryData;

        public MenuRecipeDetailModel MenuRecipeDetailModel { get; set; }

        public DeleteModel(IRecipeData recipeData, ICategoryData categoryData)
        {
            this.recipeData = recipeData;
            this.categoryData = categoryData;
        }

        public IActionResult OnGet(string recipeId)
        {
            var recipe = recipeData.GetById(recipeId);

            if (recipe == null)
            {
                return RedirectToPage("NotFound");
            }

            MenuRecipeDetailModel = new MenuRecipeDetailModel { Recipe = recipe, CategoryNameOfRecipe = categoryData.GetById(recipe.CategoryId)?.Name };

            return Page();
        }

        public IActionResult OnPost(string recipeId)
        {
            var recipe = recipeData.Delete(recipeId);

            if (recipe == null)
            {
                TempData["Message"] = $"Error deleting {recipe.Name}";
            }
            else
            {
                TempData["Message"] = $"{recipe.Name} deleted";
            }

            return RedirectToPage("List");
        }
    }
}
