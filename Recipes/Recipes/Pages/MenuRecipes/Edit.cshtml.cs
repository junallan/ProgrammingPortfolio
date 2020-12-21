using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.CodeAnalysis;
using Recipes.Core;
using Recipes.Data;

namespace Recipes.Pages.MenuRecipes
{
    public class EditModel : PageModel
    {
        private readonly IRecipeData recipeData;
        private readonly ICategoryData categoryData;
        private readonly IHtmlHelper htmlHelper;
       
        [BindProperty]
        public Recipe Recipe { get; set; }
        //[BindProperty]
        //public string CategoryId { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }

        public string FormTitle { get; set; }
        public enum Action
        {
            Adding,
            Editing
        }

        [BindProperty]
        public string Message { get; set; }

        public EditModel(IRecipeData recipeData, ICategoryData categoryData, IHtmlHelper htmlHelper)
        {
            this.recipeData = recipeData;
            this.categoryData = categoryData;
            this.htmlHelper = htmlHelper;
        }



        public IActionResult OnGetAddIngredient(string recipeId, string ingredientToAdd)
        { 
            RetrieveRecipe(recipeId, string.Empty);

            if (Recipe.Ingredients.Contains(ingredientToAdd))
            {
                Message = $"Ingredient ({ingredientToAdd}) was not added.  It already exists.";
            }
            else
            {
                Recipe.Ingredients.Add(ingredientToAdd);

                Recipe = recipeData.Update(Recipe);

                Message = $"Ingredient ({ingredientToAdd}) added.";
            }
            return new JsonResult(Message);
        }

        public IActionResult OnGetAddDirection(string recipeId, string directionToAdd)
        {
            RetrieveRecipe(recipeId, string.Empty);

            if (Recipe.Directions.Contains(directionToAdd))
            {
                Message = $"Direction ({directionToAdd}) was not added.  It already exists.";
            }
            else
            {
                Recipe.Directions.Add(directionToAdd);

                Recipe = recipeData.Update(Recipe);

                Message = $"Direction ({directionToAdd}) added.";
            }
            return new JsonResult(Message);
        }

        public IActionResult OnGetDeleteIngredient(string recipeId, string item)
        {
            RetrieveRecipe(recipeId, string.Empty);

            var isItemRemoved = Recipe.Ingredients.Remove(item);


            if (!isItemRemoved)
            {
                Message = $"Error in removing Ingredient ({item}).";
            }
            else
            {
                Recipe = recipeData.Update(Recipe);

                if(Recipe == null)
                {
                    Message = $"Error in removing Ingredient ({item}).";
                }
                else
                {
                    Message = $"Ingredient ({item}) removed.";
                }
            }
            return new JsonResult(Message);
        }

        public IActionResult OnGetDeleteDirection(string recipeId, string item)
        {
            RetrieveRecipe(recipeId, string.Empty);

            var isItemRemoved = Recipe.Directions.Remove(item);


            if (!isItemRemoved)
            {
                Message = $"Error in removing Direction ({item}).";
            }
            else
            {
                Recipe = recipeData.Update(Recipe);

                if (Recipe == null)
                {
                    Message = $"Error in removing Direction ({item}).";
                }
                else
                {
                    Message = $"Direction ({item}) removed.";
                }
            }
            return new JsonResult(Message);
        }

        public IActionResult OnPost(string IngredientOriginal, string Ingredients, string DirectionOriginal, string Directions, Recipe recipe, string categoryId)
        {
            RetrieveRecipe(recipe.Id, string.Empty);
            SetUpdatedRecipe(recipe);

            bool isUpdatedMainRecipeInfo = IngredientOriginal == null && Ingredients == null && DirectionOriginal == null && Directions == null;
            bool isUpdateIngredients = IngredientOriginal != null || Ingredients != null;
            bool isUpdatedDirections = DirectionOriginal != null || Directions != null;

            if(!(isUpdatedMainRecipeInfo || isUpdateIngredients || isUpdatedDirections))
            {
                Message = "Error in updating Recipe";
                return Page();
            }

            if (isUpdateIngredients)
            {
                var ingredientEditedIndex = Recipe.Ingredients.FindIndex(ingredient => ingredient == IngredientOriginal);

                if (ingredientEditedIndex >= 0)
                {
                    Recipe.Ingredients[ingredientEditedIndex] = Ingredients;
                }
            }
            else if (isUpdatedDirections)
            {
                var directionEditedIndex = Recipe.Directions.FindIndex(direction => direction == DirectionOriginal);

                if (directionEditedIndex >= 0)
                {
                    Recipe.Directions[directionEditedIndex] = Directions;
                }
            }

            Recipe = recipeData.Update(Recipe);

            if(Recipe == null)
            {
                Message = "Error in updating Recipe";
                return Page();
            }

            Message = isUpdatedMainRecipeInfo ? "Recipe updated" : isUpdateIngredients ? "Ingredient updated" : isUpdatedDirections ? "Direction updated" : "Error in updating Recipe";

            return Page();
        }
         
        public IActionResult OnGet(string recipeId, string message)
        {
            RetrieveRecipe(recipeId, message);
    
            return Page();
        }

        private void RetrieveRecipe(string recipeId, string message)
        {
            if (string.IsNullOrEmpty(recipeId))
            {
                Recipe = new Recipe();
                Recipe.Ingredients = new List<string>();
                Recipe.Directions = new List<string>();
                FormTitle = $" {Action.Adding.ToString()} Recipe";
            }
            else
            {
                Recipe = recipeData.GetById(recipeId);
                FormTitle = $"{Action.Editing.ToString()} {Recipe.Name}";
                Message = message;
            }

            Categories = this.categoryData.GetAll().Select(c => new SelectListItem { Value = c.Id, Text = c.Name }).ToList();
        }

        private void SetUpdatedRecipe(Recipe updatedRecipe)
        {
            Recipe.Name = updatedRecipe.Name;
            Recipe.CookTimeMinutes = updatedRecipe.CookTimeMinutes;
            Recipe.Servings = updatedRecipe.Servings;
            Recipe.CategoryId = updatedRecipe.CategoryId;
        }

        //public IActionResult OnPost()
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (string.IsNullOrEmpty(Category.Id))
        //        {
        //            Category = categoryData.Add(Category);
        //        }
        //        else
        //        {
        //            var categoryBeforeUpdate = categoryData.GetById(Category.Id);
        //            if (categoryBeforeUpdate == null) { return RedirectToPage("./NotFound"); }

        //            Category = categoryData.Update(Category);

        //            if (Category == null) { return RedirectToPage("./NotFound"); }
        //        }

        //        return RedirectToPage("./Detail", new { categoryId = Category.Id });
        //    }
        //    else
        //    {
        //        RetrieveCategory(Category.Id);
        //    }

        //    return Page();
        //}
    }
}
