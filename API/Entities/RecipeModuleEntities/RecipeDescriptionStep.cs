using System.ComponentModel.DataAnnotations;

namespace API.Entities.RecipeModuleEntities
{
    public class RecipeDescriptionStep
    {
        [Key]
        public int DescriptionStepId { get; set; }

        public int RecipeId { get; set; }

        public int OrderNumber { get; set; }

        public string Description { get; set; }
    }
}