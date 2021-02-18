using System;
using System.Collections.Generic;
using System.Text;

namespace Recipes.Data
{
    public class CollectionMappings
    {
        public static string RecipeCollectionName => nameof(CollectionMappings.Collections.Recipes);
        public static string CategoryCollectionName => nameof(CollectionMappings.Collections.Categories);

        public enum Collections
        {
            Recipes,
            Categories
        }

        public enum CategoryFields
        {
            Name
        }

        public enum RecipeFields
        {
            CookTimeMinutes,
            Servings,
            Name,
            Ingredients,
            CategoryId
        }
    }
}
