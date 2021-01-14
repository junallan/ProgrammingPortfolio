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



        public void OnGet()
        {
            var test = Ingredients;

            //TODO: Add method on IRecipeData for search by Ingredients input
            Recipes = recipeData.GetAll();
        }

        public void OnPost()
        {

        }
    }
}
