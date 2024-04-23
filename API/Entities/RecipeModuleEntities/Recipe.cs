namespace API.Entities
{
    public class Recipe
    {
        public int RecipeId { get; set; }

        public string Name { get; set; }

        public ICollection<RecipeIngredient> RecipeIngredients { get; set; }

        public int UserId { get; set; }
        
    }
}