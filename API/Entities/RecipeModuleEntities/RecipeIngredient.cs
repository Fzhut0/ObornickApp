namespace API.Entities
{
    public class RecipeIngredient
    {
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        public int IngredientId { get; set; }

        public string IngredientQuantity { get; set; }
        public Ingredient Ingredient { get; set; }
    }
}