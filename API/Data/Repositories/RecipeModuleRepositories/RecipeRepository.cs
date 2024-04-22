using API.Entities;
using API.Entities.RecipeModuleEntities;
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

        public async Task AddRecipe(Recipe recipe)
        {
            await _context.Recipes.AddAsync(recipe);
        }

        public async Task<bool> RecipeExists(string dishName)
        {
            return await _context.Recipes.AnyAsync(x => x.Name == dishName);
        }

        public async Task<IEnumerable<Recipe>> GetRecipesByIngredientId(int ingredientId)
        {
            var recipes = await _context.Recipes
                .Where(r => r.RecipeIngredients.Any(ri => ri.IngredientId == ingredientId))
                .ToListAsync();

            return recipes;
        }

        public async Task<Recipe> GetRecipeByName(string name, int userId)
        {
            return await _context.Recipes.Include(r => r.RecipeIngredients).Include(d => d.RecipeDescriptionSteps).FirstOrDefaultAsync(x => x.Name == name && x.UserId == userId);
        }

        public async Task<Recipe> GetRecipeById(int id)
        {
            return await _context.Recipes.Include(r => r.RecipeIngredients).FirstOrDefaultAsync(x => x.RecipeId == id);
        }

        public async Task<List<Recipe>> GetAllRecipes()
        {
            return await _context.Recipes.Include(r => r.RecipeIngredients).Include(d => d.RecipeDescriptionSteps).ToListAsync();
        }

        public void DeleteRecipe(Recipe recipe)
        {
            _context.Recipes.Remove(recipe);
        }

        public async Task<ICollection<Recipe>> GetUserRecipes(int userId)
        {
           return await _context.Recipes.Include(r => r.RecipeIngredients).Where(r => r.UserId == userId).ToListAsync();
        }

        public async Task<ICollection<RecipeDescriptionStep>> GetRecipeDescriptionSteps(int recipeId)
        {
            return await _context.RecipeDescriptionSteps.Where(r => r.RecipeId == recipeId).ToListAsync();
        }
    }
}