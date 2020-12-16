using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbCRUD;
using Recipes.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Recipes.Data
{
    public interface IRecipeData
    {
        Recipe Add(Recipe newRecipe);
        IEnumerable<Recipe> GetAll();
        Recipe GetById(string Id);
        Recipe Update(Recipe updatedRecipe);

    }

    public class RecipesModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public int CookTimeMinutes { get; set; }
        public int Servings { get; set; }
        public List<string> Ingredients { get; set; }
        public List<string> Directions { get; set; }
        //[BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CategoryId { get; set; }
    }

    public class MongoRecipeData : IRecipeData
    {
        private MongoDatabase _db;
        public MongoRecipeData()
        {
            _db = new MongoDatabase("Recipes");
        }

        public IEnumerable<Recipe> GetAll()
        {
            var recs = _db.LoadRecords<RecipesModel>("Recipes");
            return recs.Select(x => new Recipe { Id = x.Id, Name = x.Name, CookTimeMinutes = x.CookTimeMinutes, Servings = x.Servings, Ingredients = x.Ingredients, Directions = x.Directions, CategoryId = x.CategoryId });
        }

        public Recipe GetById(string Id)
        {
            var rec = _db.LoadRecordById<Recipe>("Recipes", Id);
            return rec;
        }

        public Recipe Update(Recipe updatedRecipe)
        {
            //return null;
            var rec = GetById(updatedRecipe.Id);

            rec.Name = updatedRecipe.Name;
            rec.CookTimeMinutes = updatedRecipe.CookTimeMinutes;
            rec.Servings = updatedRecipe.Servings;
            rec.CategoryId = updatedRecipe.CategoryId;
            rec.Ingredients = updatedRecipe.Ingredients;

            _db.UpsertRecord<Recipe>("Recipes", rec.Id, rec);

            rec = GetById(updatedRecipe.Id);
            return rec;
        }

        Recipe IRecipeData.Add(Recipe newRecipe)
        {
            //return null;
            newRecipe.Id = ObjectId.GenerateNewId().ToString();
            var recipeAdded = _db.UpsertRecord<Recipe>("Recipes", newRecipe.Id, newRecipe);
            return recipeAdded;
        }

        public Recipe Delete(string id)
        {
            return null;
            //var category = GetById(id);
            //var isDeletedCategory = _db.DeleteRecord<Category>("Categories", id);
            //return isDeletedCategory ? category : null;
        }
    }

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

        public Recipe GetById(string Id)
        {
            return recipes.Where(x => x.Id == Id).SingleOrDefault();
        }

        public Recipe Update(Recipe updatedRecipe)
        {
            throw new NotImplementedException();
        }

        Recipe IRecipeData.Add(Recipe newRecipe)
        {
            recipes.Add(newRecipe);

            return newRecipe;
        }
    }
}
