namespace API.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }

        IRecipeRepository RecipeRepository{ get; }

        IIngredientRepository IngredientRepository { get; }

        Task<bool> Complete();

        bool HasChanges();
    }
}