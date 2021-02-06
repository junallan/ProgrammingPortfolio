using System;
using System.Collections.Generic;
using System.Text;

namespace Recipes.Data
{
    public class CollectionMappings
    {
        public static string RecipeTableName => nameof(CollectionMappings.Collections.Recipes);
        public static string CategoryTableName => nameof(CollectionMappings.Collections.Categories);

        public enum Collections
        {
            Recipes,
            Categories
        }

        public enum RecipeFields
        {
            CookTimeMinutes,
            Name,
            Ingredients,
            CategoryId
        }
    }
}
