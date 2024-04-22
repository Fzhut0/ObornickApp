using API.Entities.CheckLaterLinksModuleEntities;

namespace API.Interfaces.CheckLaterLinksModuleInterfaces
{
    public interface ICheckLaterLinkCategoryRepository
    {
        Task<bool> CategoryExists(int categoryId, int userId);

        Task AddCategory(CheckLaterLinkCategory checkLaterLinkCategory);

        Task<CheckLaterLinkCategory> GetCategoryByName(string name, int userId);

        Task<CheckLaterLinkCategory> GetCategoryById(int id, int userId);

        Task<ICollection<CheckLaterLinkCategory>> GetAllCategories(int userId);

        void DeleteCategory(CheckLaterLinkCategory checkLaterLinkCategory);

        Task<CheckLaterLinkCategory> GetUserDefaultCategory(int userId);

        Task AddSubcategory(CheckLaterLinkCategory checkLaterLinkCategory, int parentCategoryId);

        Task<ICollection<CheckLaterLinkCategory>> GetSubcategories(int parentCategoryId, int userId);

    }
}