﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDbCRUD;
using Recipes.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Recipes.Data
{
    public interface IRecipeData : IBaseData<Recipe>
    {
        public string CollectionName => CollectionMappings.RecipeCollectionName;
        List<Recipe> GetByContains(string fieldName, string value);
        List<Recipe> GetByIn(string fieldName, List<string> values);
        List<Recipe> GetByOr(FilterValue[] filters);
    }

    public enum FilterType
    {
        IN,
        EQUAL,
        LIKE,
        RANGE,
        LESS_THAN_EQUAL,
        GREATER_THAN_EQUAL
    }

    public class FilterValue
    {
        public FilterType FilterType { get; set; }
        public string ColumnName { get; set; }
        public List<string> Values { get; set; }
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

    public class MongoRecipeData : BaseData<Recipe>, IRecipeData
    {
  
        public MongoRecipeData() : base(new MongoDatabase(CollectionMappings.RecipeCollectionName), CollectionMappings.RecipeCollectionName)
        {
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
            rec.Directions = updatedRecipe.Directions;

            _db.UpsertRecord<Recipe>(CollectionMappings.RecipeCollectionName, rec.Id, rec);

            rec = GetById(updatedRecipe.Id);
            return rec;
        }

        public IEnumerable<Recipe> GetAll()
        {
            var recs = _db.LoadRecords<RecipesModel>(CollectionMappings.RecipeCollectionName);
            return recs.Select(x => new Recipe { Id = x.Id, Name = x.Name, CookTimeMinutes = x.CookTimeMinutes, Servings = x.Servings, Ingredients = x.Ingredients, Directions = x.Directions, CategoryId = x.CategoryId });
        }

        public List<Recipe> GetByContains(string fieldName, string value)
        {
            var recs = _db.LoadRecordsLike<Recipe>(CollectionMappings.RecipeCollectionName, fieldName, value);
            return recs.ToList();
        }

        public List<Recipe> GetByIn(string fieldName, List<string> values)
        {
            var recs = _db.LoadRecordsIn<Recipe>(CollectionMappings.RecipeCollectionName, fieldName, values);
            return recs.ToList();
        }


        public List<Recipe> GetByOr(FilterValue[] filters)
        {
            var filterDefinitions = new List<FilterDefinition<Recipe>>();

            for (int i=0; i < filters.Length; i++)
            {
                switch (filters[i].FilterType)
                {
                    case FilterType.IN:
                        filterDefinitions.Add(MongoDatabase.FilterDefinitionIn<Recipe>(filters[i].ColumnName, filters[i].Values));
                        break;
                    case FilterType.EQUAL:
                        filterDefinitions.Add(MongoDatabase.FilterDefinitionEqual<Recipe>(filters[i].ColumnName, filters[i].Values.ElementAt(0)));
                        break;
                    case FilterType.LIKE:
                        filterDefinitions.AddRange(MongoDatabase.FilterDefinitionLike<Recipe>(filters[i].ColumnName, filters[i].Values));
                        break;
                    case FilterType.RANGE:
                        filterDefinitions.Add(MongoDatabase.FilterDefinitionRange<Recipe,string>(filters[i].ColumnName, filters[i].Values.ElementAt(0), filters[i].Values.ElementAt(1)));
                        break;
                    case FilterType.LESS_THAN_EQUAL: 
                        filterDefinitions.Add(MongoDatabase.FilterDefinitionLessThanOrEqual<Recipe>(filters[i].ColumnName, filters[i].Values.ElementAt(0)));
                        break;
                    case FilterType.GREATER_THAN_EQUAL:
                        filterDefinitions.Add(MongoDatabase.FilterDefinitionGreaterThanOrEqual<Recipe>(filters[i].ColumnName, filters[i].Values.ElementAt(0)));
                        break;
                }
            }

            var recs = _db.LoadRecordsOr(CollectionMappings.RecipeCollectionName, filterDefinitions.ToArray());
            return recs.ToList();
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

        public Recipe Add(Recipe newRecipe)
        {
            recipes.Add(newRecipe);

            return newRecipe;
        }

        public Recipe Delete(string id)
        {
            return null;
        }

        public List<Recipe> GetBy(string fieldName, string value)
        {
            throw new NotImplementedException();
        }

        public List<Recipe> GetByContains(string fieldName, string value)
        {
            throw new NotImplementedException();
        }

        public List<Recipe> GetByIn(string fieldName, List<string> values)
        {
            throw new NotImplementedException();
        }

        public List<Recipe> GetByOr(FilterValue[] filters)
        {
            throw new NotImplementedException();
        }
    }
}
