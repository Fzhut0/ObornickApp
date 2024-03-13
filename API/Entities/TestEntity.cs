using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class TestEntity
    {
        public ICollection<Recipe> Recipes { get; set; }
    }
}