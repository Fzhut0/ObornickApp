using API.Interfaces.CheckLaterLinksModuleInterfaces;

namespace API.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }

        IRecipeRepository RecipeRepository{ get; }

        IIngredientRepository IngredientRepository { get; }

        ICheckLaterLinkRepository CheckLaterLinkRepository { get; }

        ICheckLaterLinkCategoryRepository CheckLaterLinkCategoryRepository { get; }

        Task<bool> Complete();

        bool HasChanges();
    }
}