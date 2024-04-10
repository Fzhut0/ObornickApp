using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Entities.CheckLaterLinksModuleEntities;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace API.Interfaces.CheckLaterLinksModuleInterfaces
{
    public interface ICheckLaterLinkCategoryRepository
    {
        Task<bool> CategoryExists(string name, int userId);

        Task AddCategory(CheckLaterLinkCategory checkLaterLinkCategory);

        Task<CheckLaterLinkCategory> GetCategoryByName(string name, int userId);

        Task<CheckLaterLinkCategory> GetCategoryById(int id, int userId);

        Task<ICollection<CheckLaterLinkCategory>> GetAllCategories(int userId);

        void DeleteCategory(CheckLaterLinkCategory checkLaterLinkCategory);

        Task<CheckLaterLinkCategory> GetUserDefaultCategory(int userId);

        Task AddSubcategory(CheckLaterLinkCategory checkLaterLinkCategory, string parentCategoryName);

        Task<ICollection<CheckLaterLinkCategory>> GetSubcategories(string parentCategoryName, int userId);

    }
}