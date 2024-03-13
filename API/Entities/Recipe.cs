using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class Recipe
    {

        public int RecipeId { get; set; }

        public string Name { get; set; }

        public ICollection<RecipeIngredient> RecipeIngredients { get; set; }
        
    }
}