using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Recipes.Core;
using Recipes.Data;
using Recipes.Pages.Shared.Models;

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

        
        public IActionResult OnGetFilteredSearch(MenuRecipeSearchModel menuRecipeSearchModel)
        {
            List<Recipe> recipesFiltered = new List<Recipe>();
            FilterValue[] filters = new FilterValue[menuRecipeSearchModel.CountOfFiltersApplied];

            int filterCount = 0;
            //TODO: Refactor repetition??
            if (menuRecipeSearchModel.IsCookTimeEntered)
            {
                filters[filterCount++] = new FilterValue { FilterType = FilterType.EQUAL, ColumnName = "CookTimeMinutes", Values = new List<string> { menuRecipeSearchModel.CookTimeSelected } };
            }

            if (menuRecipeSearchModel.IsRecipeEntered)
            {
                filters[filterCount++] = new FilterValue { FilterType = FilterType.LIKE, ColumnName = "Name", Values = new List<string> { menuRecipeSearchModel.RecipeNameSelected } };
            }

            if (menuRecipeSearchModel.IsIngredientsEntered)
            {
                filters[filterCount++] = new FilterValue { FilterType = FilterType.IN, ColumnName = "Ingredients", Values = menuRecipeSearchModel.IngredientsSelected };
            }

            if (menuRecipeSearchModel.IsCategoryEntered)
            {
                filters[filterCount++] = new FilterValue { FilterType = FilterType.EQUAL, ColumnName = "CategoryId", Values = new List<string> { menuRecipeSearchModel.CategorySelectedId } };
            }

            recipesFiltered = this.recipeData.GetByOr(filters);


            if (recipesFiltered.Count == 0) { return RedirectToPage("Search", new { message = "No menu recipe(s) found based on search" }); }

            Recipes = recipesFiltered;

            return Page();
        }
    }
}
