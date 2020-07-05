using Recipes.Core;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Recipes.Data
{
    public interface ICategoryData
    {
        IEnumerable<Category> GetAll();
    }

    public class InMemoryCategoryData : ICategoryData
    {
        List<Category> categories;

        public InMemoryCategoryData()
        {
            categories = new List<Category>()
            {
                new Category{Id=1, Name="Breakfasts"},
                new Category{Id=2, Name="Meat Recipes"}
            };
        }

        public IEnumerable<Category> GetAll()
        {
            return categories;
        }
    }
}
