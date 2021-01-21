using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;
using MongoDbCRUD;
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

        public IActionResult OnGetFilteredSearch(string cooktime, string recipename, List<string> ingredients)
        {
            List<Recipe> recipesFiltered = new List<Recipe>();

            //if(ingredients.Count > 0)
            //{
            //    recipesFiltered = this.recipeData.GetByIn("Ingredients", ingredients);
            //}
            //else if(!string.IsNullOrEmpty(cooktime))
            //{
            //    recipesFiltered = this.recipeData.GetBy("CookTimeMinutes", cooktime);
            //}
            //else if(!string.IsNullOrEmpty(recipename))
            //{
            //    recipesFiltered = this.recipeData.GetByContains("Name", recipename);
            //}


            //TODO: Refactor later, this ideally is lower level details that should be extracted, revist once get this to work
            //FilterDefinition<Recipe>[] filters = { MongoDatabase.FilterDefinitionIn<Recipe>("Ingredients", ingredients),
            //                                                     MongoDatabase.FilterDefinitionEqual<Recipe>("CookTimeMinutes", cooktime),
            //                                                     MongoDatabase.FilterDefinitionLike<Recipe>("Name", recipename) };

            // recipesFiltered = this.recipeData.GetByOr(filters);
            FilterValue[] filters = { 
                                        new FilterValue { FilterType = FilterType.IN, ColumnName = "Ingredients", Values = ingredients },
                                        new FilterValue { FilterType = FilterType.EQUAL, ColumnName = "CookTimeMinutes", Values = new List<string> { cooktime } },
                                        new FilterValue { FilterType = FilterType.LIKE, ColumnName = "Name", Values = new List<string>{recipename } } 
                                    };

            recipesFiltered = this.recipeData.GetByOr(filters);


            if (recipesFiltered.Count == 0) { return RedirectToPage("Search"); }

            Recipes = recipesFiltered;

            return Page();
        }
    }
}
