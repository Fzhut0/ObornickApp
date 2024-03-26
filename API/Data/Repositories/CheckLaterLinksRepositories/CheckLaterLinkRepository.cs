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

        public async Task<CheckLaterLink> GetCheckLaterLinkByName(string name)
        {
            return await _context.CheckLaterLinks.FirstOrDefaultAsync(r => r.CustomName == name);
        }

        public async Task<bool> LinkExists(string name)
        {
            return await _context.CheckLaterLinks.AnyAsync(t => t.CustomName == name);
        }
    }
}