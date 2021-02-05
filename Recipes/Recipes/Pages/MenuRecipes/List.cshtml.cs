using System;
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
 
            filterCount = ApplyFilter(menuRecipeSearchModel.IsCookTimeEntered, FilterType.EQUAL, Enum.GetName(typeof(CategoryFields), CategoryFields.CookTimeMinutes), new List<string> { menuRecipeSearchModel.CookTimeSelected?.ToString() }, filters, filterCount);
            filterCount = ApplyFilter(menuRecipeSearchModel.IsRecipeEntered, FilterType.LIKE, Enum.GetName(typeof(CategoryFields), CategoryFields.Name), new List<string> { menuRecipeSearchModel.RecipeNameSelected }, filters, filterCount);
            filterCount = ApplyFilter(menuRecipeSearchModel.IsIngredientsEntered, FilterType.LIKE, Enum.GetName(typeof(CategoryFields), CategoryFields.Ingredients), menuRecipeSearchModel.IngredientsSelected, filters, filterCount);
            filterCount = ApplyFilter(menuRecipeSearchModel.IsCategoryEntered, FilterType.EQUAL, Enum.GetName(typeof(CategoryFields), CategoryFields.CategoryId), new List<string> { menuRecipeSearchModel.CategorySelectedId }, filters, filterCount);

            recipesFiltered = this.recipeData.GetByOr(filters);


            if (recipesFiltered.Count == 0) { return RedirectToPage("Search", new { message = "No menu recipe(s) found based on search" }); }

            Recipes = recipesFiltered;

            return Page();
        }

        private static int ApplyFilter(bool isFilterEnabled, FilterType filterType, string columnName, List<string> values, FilterValue[] filters, int filterCount)
        {
            if (isFilterEnabled)
            {
                filters[filterCount++] = new FilterValue { FilterType = filterType, ColumnName = columnName, Values = values };
            }

            return filterCount;
        }
    }
}
