using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class IngredientRepository : IIngredientRepository
    {
        private readonly DataContext _context;

        public IngredientRepository(DataContext context)
        {
            _context = context;
        }

        public void AddIngredient(Ingredient ingredient)
        {
            _context.Ingredients.Add(ingredient);
        }

        public async Task<bool> IngredientExists(string name)
        {
            return await _context.Ingredients.AnyAsync(x => x.Name == name);
        }

        public async Task<Ingredient> GetIngredientById(int id)
        {
           return await _context.Ingredients.FirstOrDefaultAsync(i => i.IngredientId == id);
        }

        public async Task<Ingredient> GetIngredientByName(string name)
        {
            return await _context.Ingredients.FirstOrDefaultAsync(i => i.Name == name);
        }
    }
}