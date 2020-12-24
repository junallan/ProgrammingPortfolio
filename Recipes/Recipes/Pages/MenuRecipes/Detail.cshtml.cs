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
    public class DetailModel : PageModel
    {
        private readonly IRecipeData recipeData;
        private readonly ICategoryData categoryData;

        [BindProperty]
        public Recipe Recipe { get; set; }

        [BindProperty]
        public string CategoryNameOfRecipe { get; set; }

        public DetailModel(IRecipeData recipeData, ICategoryData categoryData)
        {
            this.recipeData = recipeData;
            this.categoryData = categoryData;
        }

        public IActionResult OnGet(string recipeId)
        {
            Recipe = recipeData.GetById(recipeId);

            if(Recipe == null)
            {
                return RedirectToPage("./NotFound");
            }

            CategoryNameOfRecipe = categoryData.GetById(Recipe.CategoryId)?.Name;

            return Page();
        }
    }
}
