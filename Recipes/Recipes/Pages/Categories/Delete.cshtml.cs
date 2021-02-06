using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Recipes.Core;
using Recipes.Data;

namespace Recipes.Pages.Categories
{
    public class DeleteModel : PageModel
    {
        private readonly ICategoryData categoryData;
        private readonly IRecipeData recipeData;

        [BindProperty]
        public string Message { get; set; }


        [BindProperty]
        public Category Category { get; set; }

        public DeleteModel(ICategoryData categoryData, IRecipeData recipeData)
        {
            this.categoryData = categoryData;
            this.recipeData = recipeData;
        }

        public IActionResult OnGet(string categoryId)
        {
            Category = categoryData.GetById(categoryId);
            if(Category == null)
            {
                return RedirectToPage("./NotFound");
            }

            var recipesInAssignedCategory = recipeData.GetBy(Enum.GetName(typeof(CollectionMappings.RecipeFields), CollectionMappings.RecipeFields.CategoryId), Category.Id);

            if(recipesInAssignedCategory.Any())
            {
                var recipeNamesToUnassignCategory = RecipeNamesToUnassignCategory(recipesInAssignedCategory);

                Message = $"Recipe category {Category.Name} has assigned recipe(s).  Make sure you are okay unassigning category with recipe(s) ({recipeNamesToUnassignCategory}) before deleting.";
            }

            return Page();
        }

        public IActionResult OnPost(string categoryId)
        {
            var recipesInAssignedCategory = recipeData.GetBy(Enum.GetName(typeof(CollectionMappings.RecipeFields), CollectionMappings.RecipeFields.CategoryId), categoryId);
            
            //TODO: COMPLETE AS TRANSACTION
            var category = categoryData.Delete(categoryId);
            UnassignCategoryForRecipes(recipesInAssignedCategory);
            //
            if (category == null)
            {
                return RedirectToPage("./NotFound");
            }

           
            string message = $"{category.Name} deleted";
            StringBuilder recipesUnassignedCategories = RecipeNamesToUnassignCategory(recipesInAssignedCategory);

            if (recipesInAssignedCategory.Any())
            {
                message = $"{message}.  Recipes unassigned categories: {recipesUnassignedCategories.ToString()}.";
            }

      

            return RedirectToPage("./List", new { Message = message });
        }

        private void UnassignCategoryForRecipes(List<Recipe> recipesInAssignedCategory)
        {
            foreach (var recipeCategoryToClear in recipesInAssignedCategory)
            {
                recipeCategoryToClear.CategoryId = null;
                recipeData.Update(recipeCategoryToClear);
            }    
        }

        private StringBuilder RecipeNamesToUnassignCategory(List<Recipe> recipesInAssignedCategory)
        {
            StringBuilder recipesUnassignedCategories = new StringBuilder();

            foreach (var recipeCategoryToClear in recipesInAssignedCategory)
            {
                if (recipesUnassignedCategories.Length > 0) { recipesUnassignedCategories.Append(","); }

                recipesUnassignedCategories.Append(recipeCategoryToClear.Name);

            }

            return recipesUnassignedCategories;
        }
    }
}
