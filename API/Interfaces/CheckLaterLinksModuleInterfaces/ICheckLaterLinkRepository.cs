using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Entities.CheckLaterLinksModuleEntities;

namespace API.Interfaces.CheckLaterLinksModuleInterfaces
{
    public interface ICheckLaterLinkRepository
    {
        Task<bool> LinkExists(string name);

        Task AddLink(CheckLaterLink checkLaterLink);

        Task<CheckLaterLink> GetCheckLaterLinkByName(string name, int userId);

        Task<CheckLaterLink> GetCheckLaterLinkById(int id);

        Task<CheckLaterLink> GetCheckLaterLinkByUrl(string url, int userId);

        void DeleteLink(CheckLaterLink checkLaterLink);
    }
}