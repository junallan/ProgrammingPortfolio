using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Recipes.Core
{
    public class Recipe
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Display(Name="Cook Time (minutes)")]
        public int CookTimeMinutes { get; set; }
        public int Servings { get; set; }
        public List<string> Ingredients { get; set; }
        public List<string> Directions { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string CategoryId { get; set; }
    }
}
