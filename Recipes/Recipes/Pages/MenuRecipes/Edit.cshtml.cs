using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public IActionResult OnGetAddIngredient(string recipeId, string ingredientToAdd, string recipeNameToAdd, string cookTimeMinutesToAdd, string servingsToAdd)
        { 
            RetrieveRecipe(recipeId, string.Empty);

            if (Recipe.Ingredients.Contains(ingredientToAdd))
            {
                Message = $"Ingredient ({ingredientToAdd}) was not added.  It already exists.";
            }
            else
            {
                if (!string.IsNullOrEmpty(recipeNameToAdd)) { Recipe.Name = recipeNameToAdd; }
                if (!string.IsNullOrEmpty(cookTimeMinutesToAdd)) { Recipe.CookTimeMinutes = int.Parse(cookTimeMinutesToAdd); }
                if (!string.IsNullOrEmpty(servingsToAdd)) { Recipe.Servings = int.Parse(servingsToAdd); }

                Recipe.Ingredients.Add(ingredientToAdd);

                Recipe = string.IsNullOrEmpty(Recipe.Id) ? recipeData.Add(Recipe) : recipeData.Update(Recipe);
                
                if(ingredientToAdd.Contains("/"))
                {
                    Message = "Ingredient added.";
                }
                else
                {
                    Message = $"Ingredient ({ingredientToAdd}) added.";

                }
            }

            return new JsonResult(new { recipeId = Recipe.Id, message = Message });
        }

        public IActionResult OnGetAddDirection(string recipeId, string directionToAdd, string recipeNameToAdd)
        {
            RetrieveRecipe(recipeId, string.Empty);

            if (Recipe.Directions.Contains(directionToAdd))
            {
                Message = $"Direction ({directionToAdd}) was not added.  It already exists.";
            }
            else
            {
                Recipe.Directions.Add(directionToAdd);

                if (!string.IsNullOrEmpty(recipeNameToAdd)) 
                { 
                    Recipe.Name = recipeNameToAdd; 
                }

                Recipe = string.IsNullOrEmpty(Recipe.Id) ? recipeData.Add(Recipe) : recipeData.Update(Recipe);

                Message = $"Direction ({directionToAdd}) added.";
            }
            return new JsonResult(new { recipeId = Recipe.Id, message = Message });
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
            SetUpdatedRecipe(recipe, categoryId);

            bool isAddRecipe = string.IsNullOrEmpty(Recipe.Id);
            bool isUpdatedMainRecipeInfo = IngredientOriginal == null && Ingredients == null && DirectionOriginal == null && Directions == null;
            bool isUpdateIngredients = IngredientOriginal != null || Ingredients != null;
            bool isUpdatedDirections = DirectionOriginal != null || Directions != null;

            if (!(isUpdatedMainRecipeInfo || isUpdateIngredients || isUpdatedDirections) && !isAddRecipe)
            {
                Message = "Error in updating Recipe";
                return Page();
            }

            var action = isAddRecipe ? Action.Adding.ToString().ToLower() : Action.Editing.ToString().ToLower();

            if (isAddRecipe)
            {
                var existingRecipes = recipeData.GetBy(nameof(CollectionMappings.RecipeFields.Name), Recipe.Name.Trim());

                if(existingRecipes.Count > 0)
                {
                    Message = $"Recipe ({Recipe.Name}) already exists.  Enter a different recipe name.";
        
                    return Page();
                }


                Recipe = recipeData.Add(Recipe);
                FormTitle = $"{Action.Editing.ToString()} {Recipe.Name}";
            }
            else
            {
                UpdateRecipe(IngredientOriginal, Ingredients, DirectionOriginal, Directions, isUpdateIngredients, isUpdatedDirections);
            }



            if (Recipe == null)
            {                
                Message = $"Error in {action} Recipe";
                return Page();
            }

            Message = isAddRecipe || isUpdatedMainRecipeInfo ? $"Completed {action} Recipe" : isUpdateIngredients ? $"Completed {action} Ingredient" : isUpdatedDirections ? $"Completed {action} Direction" : $"Error in {action} Recipe";


            return Page();
        }


        private void UpdateRecipe(string IngredientOriginal, string Ingredients, string DirectionOriginal, string Directions, bool isUpdateIngredients, bool isUpdatedDirections)
        {
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
            
            var categories = this.categoryData.GetAll().Select(c => new SelectListItem { Value = c.Id, Text = c.Name }).ToList();
            categories.Insert(0, new SelectListItem { Value = string.Empty, Text = "Select..." });
            Categories = categories;
        }

        private void SetUpdatedRecipe(Recipe updatedRecipe, string categoryId)
        {
            Recipe.Name = updatedRecipe.Name;
            Recipe.CookTimeMinutes = updatedRecipe.CookTimeMinutes;
            Recipe.Servings = updatedRecipe.Servings;
            Recipe.CategoryId = categoryId;
        }
    }
}
