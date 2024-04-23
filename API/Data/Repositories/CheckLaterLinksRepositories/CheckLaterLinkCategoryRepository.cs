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

        public async Task AddSubcategory(CheckLaterLinkCategory checkLaterLinkCategory, int parentCategoryId)
        {
            var parentC = await _context.CheckLaterLinkCategories.FirstOrDefaultAsync(t => t.CategoryId == parentCategoryId);
            parentC.Subcategories.Add(checkLaterLinkCategory);
        }

        public async Task<bool> CategoryExists(int categoryId, int userId)
        {
            
            return await _context.CheckLaterLinkCategories.AnyAsync(t => t.CategoryId == categoryId && t.UserId == userId);
        }

        public void DeleteCategory(CheckLaterLinkCategory checkLaterLinkCategory)
        {
            _context.CheckLaterLinkCategories.Remove(checkLaterLinkCategory);
        }

        public async Task<ICollection<CheckLaterLinkCategory>> GetAllCategories(int userId)
        {
            return await _context.CheckLaterLinkCategories.Include(t => t.CheckLaterLinks).Include(c => c.Subcategories).Where(u => u.UserId == userId).ToListAsync();
        }

        public async Task<CheckLaterLinkCategory> GetCategoryById(int id, int userId)
        {
            return await _context.CheckLaterLinkCategories.Include(l => l.CheckLaterLinks).Include(c => c.Subcategories).ThenInclude(sub => sub.CheckLaterLinks).FirstOrDefaultAsync(t => t.CategoryId == id && t.UserId == userId);
        }

        public async Task<CheckLaterLinkCategory> GetCategoryByName(string name, int userId)
        {
            return await _context.CheckLaterLinkCategories.Include(l => l.CheckLaterLinks).Include(c => c.Subcategories).ThenInclude(sub => sub.CheckLaterLinks).FirstOrDefaultAsync(t => t.Name == name && t.UserId == userId);
        }

        public async Task<ICollection<CheckLaterLinkCategory>> GetSubcategories(int parentCategoryId, int userId)
        {
            var parentCategory = await GetCategoryById(parentCategoryId, userId);
            return parentCategory.Subcategories;
        }

        public async Task<CheckLaterLinkCategory> GetUserDefaultCategory(int userId)
        {
            var categories = await GetAllCategories(userId);
            return categories.MinBy(c => c.CategoryId);
        }
    }
}