using MongoDB.Bson;
using Recipes.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Recipes.Data
{
    public interface IRecipeData
    {
        Recipe Add(Recipe newRecipe);
        IEnumerable<Recipe> GetAll();
    }

    //public class MongoCategoryData : ICategoryData
    //{
    //    private MongoDatabase _db;
    //    public MongoCategoryData()
    //    {
    //        _db = new MongoDatabase("Recipes");
    //    }

    //    public IEnumerable<Category> GetAll()
    //    {
    //        var recs = _db.LoadRecords<CategoriesModel>("Categories");
    //        return recs.Select(x => new Category { Id = x.Id, Name = x.Name });
    //    }

    //    public Category GetById(string Id)
    //    {
    //        var rec = _db.LoadRecordById<Category>("Categories", Id);
    //        return rec;
    //    }

    //    public Category Update(Category updatedCategory)
    //    {
    //        var rec = GetById(updatedCategory.Id);
    //        rec.Name = updatedCategory.Name;

    //        _db.UpsertRecord<Category>("Categories", rec.Id, rec);

    //        rec = GetById(updatedCategory.Id);
    //        return rec;
    //    }

    //    Category ICategoryData.Add(Category newCategory)
    //    {
    //        newCategory.Id = ObjectId.GenerateNewId().ToString();
    //        var categoryAdded = _db.UpsertRecord<Category>("Categories", newCategory.Id, newCategory);
    //        return categoryAdded;
    //    }

    //    public Category Delete(string id)
    //    {
    //        var category = GetById(id);
    //        var isDeletedCategory = _db.DeleteRecord<Category>("Categories", id);
    //        return isDeletedCategory ? category : null;
    //    }
    //}

    public class InMemoryRecipeData : IRecipeData
    {
        readonly List<Recipe> recipes;

        public InMemoryRecipeData()
        {
            recipes = new List<Recipe>()
            {
                new Recipe{ Id = ObjectId.GenerateNewId().ToString(), 
                                Name = "Potatoes Breakfast Salad", 
                                CookTimeMinutes = 15, 
                                Servings = 4, 
                                Ingredients = new List<string>{ 
                                                "6 potatoes, peeled and cubed",
                                                "1 1/2 cups water",
                                                "1 cup homemade mayonnaise",
                                                "1/4 cup onion; finely chopped",
                                                "1 tpsp. dill pickle juice",
                                                "2 tpsp. parsley; finely chopped",
                                                "1 tpsp. mustard",
                                                "4 eggs",
                                                "Salt and black pepper to the taste"},
                                Directions = new List<string>{
                                                "Put potatoes, eggs and the water in the steamer basket of your instant pot, close the lide and cook on High for 4 minutes",
                                                "Quick release the pressure, transfer eggs to a bowl filled with ice water and leave them to cool down",
                                                "In a bowl, mix mayo with pickle juice, onion, parsley and mustard and stir well",
                                                "Add potatoes and toss to coat",
                                                "Peel eggs, cho them, add them to salad and toss again.",
                                                "Add salt and pepper t the taste; stir and serve your salad with toasted bread slices."},
                                CategoryId = "5f2abad27f7947b1a07bb8a9"
                                },
                new Recipe{ Id = ObjectId.GenerateNewId().ToString(),
                                Name="Beef and Cabbage",
                                CookTimeMinutes = 90,
                                Servings = 6,
                                Ingredients = new List<string>{
                                                "2 1/2 lb. beef brisket",
                                                "4 carrots; chopped",
                                                "1 cabbage heat, cut into 6 wedges",
                                                "6 potatoes, cut into quarters",
                                                "4 cups water",
                                                "2 bay leaves",
                                                "3 garlic cloves; chopped",
                                                "3 turnips, cut into quarters",
                                                "Horseradish sauce for serving",
                                                "Salt and black pepper to the taste"},
                                Directions = new List<string>{
                                                "Put beef brisket and water in your instant pot, add salt, pepper, garlic and bay leaves, seal the intant pot lid and cook at High for 1 hour and 15 minutes.",
                                                "Quick release the pressure, carefully open the lid; add carrots, cabbage, potatoes, and turnips; then stir well.  Seal the instant pot lid again and cook at High for 6 minutes",
                                                "Release the pressure naturally, carefully open the lid; divide among plates and serve with horseradish sauce on top."},
                                CategoryId = "5f2abb417f7947b1a07bb8aa"
                }
            };
        }

        public IEnumerable<Recipe> GetAll()
        {
            return recipes;
        }

        Recipe IRecipeData.Add(Recipe newRecipe)
        {
            recipes.Add(newRecipe);

            return newRecipe;
        }
    }
}
