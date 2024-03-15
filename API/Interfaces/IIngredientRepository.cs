using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.Interfaces
{
    public interface IIngredientRepository
    {
        Task<Ingredient> GetIngredientById(int id);

        Task<Ingredient> GetIngredientByName(string name);

        Task AddIngredient(Ingredient ingredient);

        public Task<bool> IngredientExists(string name);

        Task<IEnumerable<Ingredient>> GetIngredientsForRecipe(int id);

        Task<RecipeIngredient> GetRecipeIngredientById(int id);
    }
}