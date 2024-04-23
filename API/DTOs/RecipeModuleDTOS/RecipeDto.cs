namespace API.DTOs
{
    public class RecipeDto
    {
        public string OriginalName { get; set; }
        public string Name { get; set; }
        public List<RecipeIngredientDto> Ingredients { get; set; }

        public int RecipeId { get; set; }
    }
}