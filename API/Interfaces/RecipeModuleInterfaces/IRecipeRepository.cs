using API.Entities;
using API.Entities.RecipeModuleEntities;

namespace API.Interfaces
{
    public interface IRecipeRepository
    {
        Task<bool> RecipeExists(string dishName);

        Task AddRecipe(Recipe recipe);

        Task<Recipe> GetRecipeByName(string name, int userId);

        Task<Recipe> GetRecipeById(int id);

        Task<IEnumerable<Recipe>> GetRecipesByIngredientId(int ingredientId);

        Task<List<Recipe>> GetAllRecipes();

        Task<ICollection<Recipe>> GetUserRecipes(int userId);

        Task<ICollection<RecipeDescriptionStep>> GetRecipeDescriptionSteps(int recipeId);

        void DeleteRecipe(Recipe recipe);


    }
}