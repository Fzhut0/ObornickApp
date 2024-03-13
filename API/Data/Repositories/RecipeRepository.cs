using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly DataContext _context;

        public RecipeRepository(DataContext context)
        {
            _context = context;
        }

        public void AddRecipe(Recipe recipe)
        {
            _context.Recipes.Add(recipe);
        }

        public async Task<Recipe> GetRecipe(string name)
        {
            return await _context.Recipes.Include(r => r.RecipeIngredients).FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<bool> RecipeExists(string dishName)
        {
            return await _context.Recipes.AnyAsync(x => x.Name == dishName);
        }

          public async Task<IEnumerable<Recipe>> GetRecipesByIngredientId(int ingredientId)
        {
            // Query the DbContext to retrieve recipes containing the provided ingredient ID
            var recipes = await _context.Recipes
                .Where(r => r.RecipeIngredients.Any(ri => ri.IngredientId == ingredientId))
                .ToListAsync();

            return recipes;
        }
    }
}