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
    public class ServingRangeModel
    {
        public ServingRangeItem ServingsSmall => new ServingRangeItem { LowEnd = 0, HighEnd = 2, Description = "2 or 1 with left" };
        public ServingRangeItem ServingsMedium => new ServingRangeItem { LowEnd = 3, HighEnd = 4, Description = "4 or 2-3 with leftovers" };
        public ServingRangeItem ServingsLarge => new ServingRangeItem { LowEnd = 5, HighEnd = int.MaxValue, Description = "Family of 5+" };
    }

    public class ServingRangeItem
    {
        public string Description { get; set; }
        public int LowEnd { get; set; }
        public int HighEnd { get; set; }
    }

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
        [BindProperty(SupportsGet = true)]
        public string ServingNumber { get; set; }
        public IEnumerable<SelectListItem> Servings { get; set; }

        public ServingRangeModel ServingOptions => new ServingRangeModel();

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

            var minGroupServings = new SelectListGroup { Name= ServingOptions.ServingsSmall.Description };
            var mediumGroupServings = new SelectListGroup { Name = ServingOptions.ServingsMedium.Description };
            var largeGroupServings = new SelectListGroup { Name = ServingOptions.ServingsLarge.Description };
            
            var servings = new List<SelectListItem>
            {
                new SelectListItem{ Value=string.Empty, Text=string.Empty},
                new SelectListItem { Value=ServingOptions.ServingsSmall.HighEnd.ToString(), Text=$"{ServingOptions.ServingsSmall.HighEnd.ToString()} servings", Group=minGroupServings },
                new SelectListItem { Value=ServingOptions.ServingsMedium.HighEnd.ToString(), Text=$"{ServingOptions.ServingsMedium.HighEnd.ToString()} servings", Group=mediumGroupServings },
                new SelectListItem { Value=ServingOptions.ServingsLarge.LowEnd.ToString(), Text=$"{ServingOptions.ServingsLarge.LowEnd.ToString()} servings", Group=largeGroupServings }
            };

            Servings = servings;

            if(IsSearchValidationError(message))
            {
                return Page();
            }


            var model = new MenuRecipeSearchModel { CookTimeSelected = CookTime, ServingsSelected = ServingNumber, RecipeNameSelected = RecipeName, IngredientsSelected = Ingredients?.Split(",").ToList(), CategorySelectedId = CategoryId };

            return RedirectToPage("List", "FilteredSearch", model);
        }

        private bool IsSearchValidationError(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                Message = message;

                return true;
            }
            else if (!CookTime.HasValue && string.IsNullOrEmpty(RecipeName) && string.IsNullOrEmpty(Ingredients) && string.IsNullOrEmpty(CategoryId) && string.IsNullOrEmpty(ServingNumber))
            {
                Message = "Enter criteria to search for recipe(s)";

                return true;
            }

            return false;
        }
    }
}
