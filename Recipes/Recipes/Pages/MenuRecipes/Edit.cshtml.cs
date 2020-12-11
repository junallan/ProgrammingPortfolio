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
                Message = $"Ingredient ({ingredientToAdd}) added.";
            }
            return new JsonResult(Message);
        }
    
        public IActionResult OnPost(string recipeId, string IngredientOriginal, string Ingredients)
        {
            RetrieveRecipe(recipeId, string.Empty);
            Categories = this.categoryData.GetAll().Select(c => new SelectListItem { Value = c.Id, Text = c.Name }).ToList();
         
           
           var ingredientEditedIndex = Recipe.Ingredients.FindIndex(ingredient => ingredient == IngredientOriginal); 
            
            if(ingredientEditedIndex >= 0)
            {
                Recipe.Ingredients[ingredientEditedIndex] = Ingredients;
            }
           
      
            return Page();
        }
         
        public IActionResult OnGet(string recipeId, string message)
        {
            RetrieveRecipe(recipeId, message);
            Categories = this.categoryData.GetAll().Select(c => new SelectListItem { Value = c.Id, Text = c.Name }).ToList();
           // Message = "Test";
            //CategoryId = Recipe.CategoryId;
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
