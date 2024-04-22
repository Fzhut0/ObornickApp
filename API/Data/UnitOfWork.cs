using API.Data.Repositories;
using API.Data.Repositories.CheckLaterLinksRepositories;
using API.Interfaces;
using API.Interfaces.CheckLaterLinksModuleInterfaces;
using AutoMapper;

namespace API.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public UnitOfWork(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public IUserRepository UserRepository => new UserRepository(_context, _mapper);

        public IRecipeRepository RecipeRepository => new RecipeRepository(_context);

        public IIngredientRepository IngredientRepository => new IngredientRepository(_context);

        public ICheckLaterLinkRepository CheckLaterLinkRepository => new CheckLaterLinkRepository(_context);

        public ICheckLaterLinkCategoryRepository CheckLaterLinkCategoryRepository => new CheckLaterLinkCategoryRepository(_context);

        public async Task<bool> Complete()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }
    }
}