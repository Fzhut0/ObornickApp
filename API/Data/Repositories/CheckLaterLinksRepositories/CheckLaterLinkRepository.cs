using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities.CheckLaterLinksModuleEntities;
using API.Interfaces.CheckLaterLinksModuleInterfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories.CheckLaterLinksRepositories
{
    public class CheckLaterLinkRepository : ICheckLaterLinkRepository
    {
        private readonly DataContext _context;

        public CheckLaterLinkRepository(DataContext context)
        {
            _context = context;
        }

        public async Task AddLink(CheckLaterLink checkLaterLink)
        {
            await _context.CheckLaterLinks.AddAsync(checkLaterLink);
        }

        public void DeleteLink(CheckLaterLink checkLaterLink)
        {
            _context.CheckLaterLinks.Remove(checkLaterLink);
        }

        public async Task<CheckLaterLink> GetCheckLaterLinkById(int id)
        {
            return await _context.CheckLaterLinks.FirstOrDefaultAsync(r => r.LinkId == id);
        }

        public async Task<CheckLaterLink> GetCheckLaterLinkByName(string name, int userId)
        {
            return await _context.CheckLaterLinks.FirstOrDefaultAsync(r => r.CustomName == name && r.UserId == userId);
        }

        public async Task<CheckLaterLink> GetCheckLaterLinkByUrl(string url, int userId)
        {
            return await _context.CheckLaterLinks.FirstOrDefaultAsync(r => r.SavedUrl == url && r.UserId == userId);
        }

        public async Task<bool> LinkExists(string url)
        {
            return await _context.CheckLaterLinks.AnyAsync(t => t.SavedUrl == url);
        }
    }
}