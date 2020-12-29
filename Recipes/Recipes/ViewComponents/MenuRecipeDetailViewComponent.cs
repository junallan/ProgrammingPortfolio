using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Recipes.Core;
using Recipes.Data;

namespace Recipes.ViewComponents
{
    public class MenuRecipeDetailViewComponent : ViewComponent
    {
        private readonly IRecipeData recipeData;
        private readonly ICategoryData categoryData;

        public MenuRecipeDetailViewComponent(IRecipeData recipeData, ICategoryData categoryData)
        {
            this.recipeData = recipeData;
            this.categoryData = categoryData;
        }

        public IViewComponentResult Invoke(/*string recipeId*/MenuRecipeDetailModel model)
        {
            return View(model);
        }
    }

    public class MenuRecipeDetailModel
    {
        public Recipe Recipe { get; set; }
        public string CategoryNameOfRecipe { get; set; }
    }
}
