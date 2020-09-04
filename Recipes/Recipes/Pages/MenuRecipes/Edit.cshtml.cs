using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public EditModel(IRecipeData recipeData, ICategoryData categoryData, IHtmlHelper htmlHelper)
        {
            this.recipeData = recipeData;
            this.categoryData = categoryData;
            this.htmlHelper = htmlHelper;
        }

        public IActionResult OnGet(string recipeId)
        {
            RetrieveRecipe(recipeId);
            Categories = this.categoryData.GetAll().Select(c => new SelectListItem { Value = c.Id, Text = c.Name }).ToList();
            //Recipe.CategoryId;.
            return Page();
        }

        private void RetrieveRecipe(string recipeId)
        {
            if (string.IsNullOrEmpty(recipeId))
            {
                Recipe = new Recipe();
                FormTitle = $" {Action.Adding.ToString()} Recipe";
            }
            else
            {
                Recipe = recipeData.GetById(recipeId);
                FormTitle = $"{Action.Editing.ToString()} {Recipe.Name}";
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
