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

        Task AddRecipe(Recipe recipe);

        Task<Recipe> GetRecipeByName(string name);

        Task<Recipe> GetRecipeById(int id);

        public Task<IEnumerable<Recipe>> GetRecipesByIngredientId(int ingredientId);

        Task<List<Recipe>> GetAllRecipes();
    }
}