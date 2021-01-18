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
    public class SearchModel : PageModel
    {
        private readonly IRecipeData recipeData;
        public IEnumerable<Recipe> Recipes { get; set; }

        public SearchModel(IRecipeData recipeData)
        {
            this.recipeData = recipeData;
        }

        [BindProperty(SupportsGet = true)]
        public string Ingredients { get; set; }
        [BindProperty(SupportsGet = true)]
        public string CookTime { get; set; }
        [BindProperty(SupportsGet = true)]
        public string RecipeName { get; set; }

        public IActionResult OnGet()
        {
            //var test = Ingredients;
            //var test2 = CookTime;

            //TODO: Add method on IRecipeData for search by Ingredients input
            //Recipes = recipeData.GetAll();
            if (string.IsNullOrEmpty(CookTime) && string.IsNullOrEmpty(RecipeName)) { return Page(); }


            return RedirectToPage("List", "FilteredSearch", new { cooktime = CookTime, recipename = RecipeName });
        }

        public void OnPost()
        {

        }
    }
}