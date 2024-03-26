using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.DTOs
{
    public class RecipeDto
    {
        public string OldName { get; set; }
        public string Name { get; set; }
        public List<RecipeIngredientDto> Ingredients { get; set; }
    }
}