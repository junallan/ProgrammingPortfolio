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
    public class ListModel : PageModel
    {
        private readonly IRecipeData recipeData;

        public IEnumerable<Recipe> Recipes { get; set; }

        public ListModel(IRecipeData recipeData)
        {
            this.recipeData = recipeData;
        }
        public void OnGet()
        {
            Recipes = recipeData.GetAll();
        }

        public IActionResult OnGetFilteredSearch(string cooktime)
        {
            var recipesFiltered = this.recipeData.GetBy("CookTimeMinutes", cooktime);

            if (recipesFiltered.Count == 0) { return RedirectToPage("Search"); }

            Recipes = recipesFiltered;

            return Page();
        }
    }
}
