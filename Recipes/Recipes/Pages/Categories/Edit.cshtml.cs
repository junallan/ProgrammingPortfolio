﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Bson;
using Recipes.Core;
using Recipes.Data;

namespace Recipes.Pages.Categories
{
    public class EditModel : PageModel
    {
        private readonly ICategoryData categoryData;

        [BindProperty]
        public Category Category { get; set; }
        [BindProperty]
        public string Message { get; set; }
        public string FormTitle { get; set; }
        public enum Action
        {
            Adding,
            Editing
        }

        public EditModel(ICategoryData categoryData)
        {
            this.categoryData = categoryData;
        }

        public IActionResult OnGet(string categoryId)
        {
            RetrieveCategory(categoryId);

            return Page();
        }

        private void RetrieveCategory(string categoryId)
        {
            if (string.IsNullOrEmpty(categoryId))
            {
                Category = new Category();
                FormTitle = $" {Action.Adding.ToString()} Category";
            }
            else
            {
                Category = categoryData.GetById(categoryId);
                FormTitle = $"{Action.Editing.ToString()} {Category.Name}";
            }
        }

        public IActionResult OnPost()
        {
            if(ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(Category.Id))
                {
                    var existingCategories = categoryData.GetBy(nameof(CollectionMappings.CategoryFields.Name), Category.Name.Trim());
                    if(existingCategories.Count > 0)
                    {
                        Message = $"Category ({Category.Name}) already exists.  Enter a different cateogry name.";
                        return Page();
                    }
                    Category = categoryData.Add(Category);
                    return RedirectToPage("List", new { Message = $"Category {Category.Name} added" });
                }
                else
                {
                    var categoryBeforeUpdate = categoryData.GetById(Category.Id);
                    if (categoryBeforeUpdate == null) { return RedirectToPage("./NotFound"); }

                    Category = categoryData.Update(Category);

                    if (Category == null) { return RedirectToPage("./NotFound"); }
                }

                return RedirectToPage("./Detail", new { categoryId = Category.Id });
            }
            else
            {
                RetrieveCategory(Category.Id);
            }

            return Page();
        }




    }
}