using System.Collections.Generic;
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

        public IActionResult OnGetFilteredSearch(string cooktime, string recipename, List<string> ingredients, string categorySelectedId)
        {
            List<Recipe> recipesFiltered = new List<Recipe>();

            bool isSearchByCooktime = !string.IsNullOrEmpty(cooktime);
            bool isSearchByRecipeName = !string.IsNullOrEmpty(recipename);
            bool isSearchByIngredients = ingredients.Count > 0;
            bool isCategorySelected = !string.IsNullOrEmpty(categorySelectedId);

            int filtersCount = (isSearchByCooktime ? 1 : 0) + (isSearchByRecipeName ? 1 : 0) + (isSearchByIngredients ? 1 : 0) + (isCategorySelected ? 1 : 0);
            FilterValue[] filters = new FilterValue[filtersCount];

            int filterCount = 0;
            if(isSearchByCooktime)
            {
                filters[filterCount++] = new FilterValue { FilterType = FilterType.EQUAL, ColumnName = "CookTimeMinutes", Values = new List<string> { cooktime } };              
            }

            if (isSearchByRecipeName)
            {
                filters[filterCount++] = new FilterValue { FilterType = FilterType.LIKE, ColumnName = "Name", Values = new List<string> { recipename } };
            }

            if (isSearchByIngredients)
            {
                filters[filterCount++] = new FilterValue { FilterType = FilterType.IN, ColumnName = "Ingredients", Values = ingredients };
            }

            if (isCategorySelected)
            {
                filters[filterCount++] = new FilterValue { FilterType = FilterType.EQUAL, ColumnName = "CategoryId", Values = new List<string> { categorySelectedId } };
            }

            recipesFiltered = this.recipeData.GetByOr(filters);


            if (recipesFiltered.Count == 0) { return RedirectToPage("Search"); }

            Recipes = recipesFiltered;

            return Page();
        }
    }
}
