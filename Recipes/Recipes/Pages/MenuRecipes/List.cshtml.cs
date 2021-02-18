using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Recipes.Core;
using Recipes.Data;
using Recipes.Pages.Shared.Models;

namespace Recipes.Pages.MenuRecipes
{
    public class RecipeModel
    {
        public string RecipeId { get; set; }
        public string RecipeName { get; set; }
        public string CategoryName { get; set; }
    }

    public class ListModel : PageModel
    {
        private readonly IRecipeData recipeData;
        private readonly ICategoryData categoryData;

        public IEnumerable<RecipeModel> Recipes { get; set; }

        public ListModel(IRecipeData recipeData, ICategoryData categoryData)
        {
            this.recipeData = recipeData;
            this.categoryData = categoryData;
        }
        public void OnGet()
        {
            Recipes = recipeData.GetAll().Select(x => new RecipeModel { RecipeId = x.Id, CategoryName = this.categoryData.GetById(x.CategoryId)?.Name, RecipeName = x.Name }).OrderBy(x => x.CategoryName).ThenBy(x => x.RecipeName);
        }

        
        public IActionResult OnGetFilteredSearch(MenuRecipeSearchModel menuRecipeSearchModel)
        {
            List<Recipe> recipesFiltered = new List<Recipe>();
            FilterValue[] filters = new FilterValue[menuRecipeSearchModel.CountOfFiltersApplied];

            int filterCount = 0;
 
            filterCount = ApplyFilter(menuRecipeSearchModel.IsCookTimeEntered, FilterType.EQUAL, Enum.GetName(typeof(CollectionMappings.RecipeFields), CollectionMappings.RecipeFields.CookTimeMinutes), new List<string> { menuRecipeSearchModel.CookTimeSelected?.ToString() }, filters, filterCount);
            filterCount = ApplyFilter(menuRecipeSearchModel.IsRecipeEntered, FilterType.LIKE, Enum.GetName(typeof(CollectionMappings.RecipeFields), CollectionMappings.RecipeFields.Name), new List<string> { menuRecipeSearchModel.RecipeNameSelected }, filters, filterCount);
            filterCount = ApplyFilter(menuRecipeSearchModel.IsIngredientsEntered, FilterType.LIKE, Enum.GetName(typeof(CollectionMappings.RecipeFields), CollectionMappings.RecipeFields.Ingredients), menuRecipeSearchModel.IngredientsSelected, filters, filterCount);
            filterCount = ApplyFilter(menuRecipeSearchModel.IsCategoryEntered, FilterType.EQUAL, Enum.GetName(typeof(CollectionMappings.RecipeFields), CollectionMappings.RecipeFields.CategoryId), new List<string> { menuRecipeSearchModel.CategorySelectedId }, filters, filterCount);

            //TODO: MONGO CRUD for range filter, less than, greater than
            //filterCount = ApplyFilter(menuRecipeSearchModel.IsServingsSelected, FilterType.EQUAL, Enum.GetName(typeof(CollectionMappings.RecipeFields), CollectionMappings.RecipeFields.Servings), new List<string> { menuRecipeSearchModel.ServingsSelected }, filters, filterCount);


            recipesFiltered = this.recipeData.GetByOr(filters);


            if (recipesFiltered.Count == 0) { return RedirectToPage("Search", new { message = "No menu recipe(s) found based on search" }); }

            Recipes = recipesFiltered.Select(x => new RecipeModel { RecipeId = x.Id, CategoryName = this.categoryData.GetById(x.CategoryId)?.Name, RecipeName = x.Name }).OrderBy(x => x.CategoryName).ThenBy(x => x.RecipeName); 

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
