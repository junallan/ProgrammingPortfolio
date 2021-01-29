using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Recipes.Core;
using Recipes.Data;
using Recipes.Pages.Shared.Models;

namespace Recipes.Pages.MenuRecipes
{
    public class SearchModel : PageModel
    {
        private readonly IRecipeData recipeData;
        private readonly ICategoryData categoryData;

        public IEnumerable<Recipe> Recipes { get; set; }

        [BindProperty]
        public string Message { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Ingredients { get; set; }
        [BindProperty(SupportsGet = true)]
        public string CategoryId { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? CookTime { get; set; }
        [BindProperty(SupportsGet = true)]
        public string RecipeName { get; set; }

        public SearchModel(IRecipeData recipeData, ICategoryData categoryData)
        {
            this.recipeData = recipeData;
            this.categoryData = categoryData;
        }
        public IActionResult OnGet(string message)
        {
            var categories = this.categoryData.GetAll().Select(c => new SelectListItem { Value = c.Id, Text = c.Name }).ToList();
            categories.Insert(0, new SelectListItem { Value = string.Empty, Text = string.Empty });

            Categories = categories;
          
            if(IsSearchValidationError(message))
            {
                return Page();
            }

            var model = new MenuRecipeSearchModel { CookTimeSelected = CookTime, RecipeNameSelected = RecipeName, IngredientsSelected = Ingredients?.Split(",").ToList(), CategorySelectedId = CategoryId };
            return RedirectToPage("List", "FilteredSearch", model);
        }

        private bool IsSearchValidationError(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                Message = message;

                return true;
            }
            else if (!CookTime.HasValue && string.IsNullOrEmpty(RecipeName) && string.IsNullOrEmpty(Ingredients) && string.IsNullOrEmpty(CategoryId))
            {
                Message = "Enter criteria to search for recipe(s)";

                return true;
            }

            return false;
        }
    }
}
