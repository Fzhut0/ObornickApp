using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities.CheckLaterLinksModuleEntities;
using API.Interfaces.CheckLaterLinksModuleInterfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories.CheckLaterLinksRepositories
{
    public class CheckLaterLinkCategoryRepository : ICheckLaterLinkCategoryRepository
    {
        private readonly DataContext _context;

        public CheckLaterLinkCategoryRepository(DataContext context)
        {
            _context = context;
        }

        public async Task AddCategory(CheckLaterLinkCategory checkLaterLinkCategory)
        {
            await _context.CheckLaterLinkCategories.AddAsync(checkLaterLinkCategory);
        }

        public async Task<bool> CategoryExists(string name)
        {
            return await _context.CheckLaterLinkCategories.AnyAsync(t => t.Name == name);
        }

        public void DeleteCategory(CheckLaterLinkCategory checkLaterLinkCategory)
        {
            _context.CheckLaterLinkCategories.Remove(checkLaterLinkCategory);
        }

        public async Task<ICollection<CheckLaterLinkCategory>> GetAllCategories()
        {
            return await _context.CheckLaterLinkCategories.Include(t => t.CheckLaterLinks).ToListAsync();
        }

        public async Task<CheckLaterLinkCategory> GetCategoryByName(string name)
        {
            return await _context.CheckLaterLinkCategories.FirstOrDefaultAsync(t => t.Name == name);
        }
    }
}