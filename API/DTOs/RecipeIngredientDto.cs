using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class RecipeIngredientDto
    {
       // public int IngredientId { get; set; }

        public string IngredientName { get; set; }
        public int Quantity { get; set; }
    }
}