using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities.CheckLaterLinksModuleEntities;

namespace API.Interfaces.CheckLaterLinksModuleInterfaces
{
    public interface ICheckLaterLinkCategoryRepository
    {
        Task<bool> CategoryExists(string name);

        Task AddCategory(CheckLaterLinkCategory checkLaterLinkCategory);

        Task<CheckLaterLinkCategory> GetCategoryByName(string name);
    }
}