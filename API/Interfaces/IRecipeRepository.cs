using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IRecipeRepository
    {
        public Task<bool> RecipeExists(string dishName);

        void AddRecipe(Recipe recipe);

        Task<Recipe> GetRecipe(string name);

        public Task<IEnumerable<Recipe>> GetRecipesByIngredientId(int ingredientId);
    }
}