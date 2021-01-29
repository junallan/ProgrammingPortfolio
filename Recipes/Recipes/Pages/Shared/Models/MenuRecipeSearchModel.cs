using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.Pages.Shared.Models
{
    public class MenuRecipeSearchModel
    {
        public int? CookTimeSelected { get; set; }
        public string RecipeNameSelected { get; set; }
        public List<string> IngredientsSelected { get; set; }
        public string CategorySelectedId { get; set; }
        public bool IsCookTimeEntered => CookTimeSelected.HasValue;
        public bool IsRecipeEntered => !string.IsNullOrEmpty(RecipeNameSelected);
        public bool IsIngredientsEntered => IngredientsSelected?.Count > 0;
        public bool IsCategoryEntered => !string.IsNullOrEmpty(CategorySelectedId);
        public int CountOfFiltersApplied => (IsCookTimeEntered ? 1 : 0) + (IsRecipeEntered ? 1 : 0) + (IsIngredientsEntered ? 1 : 0) + (IsCategoryEntered ? 1 : 0);
    }
}
