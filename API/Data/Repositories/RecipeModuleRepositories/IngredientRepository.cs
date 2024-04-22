using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class IngredientRepository : IIngredientRepository
    {
        private readonly DataContext _context;

        public IngredientRepository(DataContext context)
        {
            _context = context;
        }

        public async Task AddIngredient(Ingredient ingredient)
        {
           await _context.Ingredients.AddAsync(ingredient);
        }

        public async Task<bool> IngredientExists(string name)
        {
            return await _context.Ingredients.AnyAsync(x => x.Name == name);
        }

        public async Task<Ingredient> GetIngredientById(int id)
        {
           return await _context.Ingredients.FirstOrDefaultAsync(i => i.IngredientId == id);
        }

        public async Task<Ingredient> GetIngredientByName(string name)
        {
            return await _context.Ingredients.FirstOrDefaultAsync(i => i.Name.ToLower() == name.ToLower());
        }

        public async Task<IEnumerable<Ingredient>> GetIngredientsForRecipe(int recipeId)
        {

            var ingredients = await _context.Ingredients
                .Where(r => r.RecipeIngredients.Any(ri => ri.RecipeId == recipeId)).ToListAsync();

            return ingredients;
        }

        public async Task<RecipeIngredient> GetRecipeIngredientById(int id, int recipeId)
        {
            return await _context.RecipeIngredients.FirstOrDefaultAsync(r => r.IngredientId == id && r.RecipeId == recipeId);
        }
    }
}