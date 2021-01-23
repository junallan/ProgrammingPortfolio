using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Recipes.Core;
using Recipes.Data;

namespace Recipes.Pages.MenuRecipes
{
    public class SearchModel : PageModel
    {
        private readonly IRecipeData recipeData;
        private readonly ICategoryData categoryData;

        public IEnumerable<Recipe> Recipes { get; set; }

        public SearchModel(IRecipeData recipeData, ICategoryData categoryData)
        {
            this.recipeData = recipeData;
            this.categoryData = categoryData;
        }

        [BindProperty(SupportsGet = true)]
        public string Ingredients { get; set; }
        [BindProperty(SupportsGet = true)]
        public string CategoryId { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
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

            var categories = this.categoryData.GetAll().Select(c => new SelectListItem { Value = c.Id, Text = c.Name }).ToList();
            categories.Insert(0, new SelectListItem { Value = string.Empty, Text = string.Empty });

            Categories = categories;
          

            if (string.IsNullOrEmpty(CookTime) && string.IsNullOrEmpty(RecipeName) && string.IsNullOrEmpty(Ingredients) && string.IsNullOrEmpty(CategoryId)) { return Page(); }


            return RedirectToPage("List", "FilteredSearch", new { cooktime = CookTime, recipename = RecipeName, ingredients = Ingredients?.Split(","), categorySelectedId = CategoryId });
        }

        public void OnPost()
        {

        }
    }
}
