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

        [BindProperty]
        public Recipe Recipe { get; set; }

        public DetailModel(IRecipeData recipeData)
        {
            this.recipeData = recipeData;
        }

        public IActionResult OnGet(string recipeId)
        {
            Recipe = recipeData.GetById(recipeId);

            if(Recipe == null)
            {
                return RedirectToPage("./NotFound");
            }

            return Page();
        }
    }
}
