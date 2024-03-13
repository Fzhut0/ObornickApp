using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers
{
public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<AppUser, UserDto>();
        CreateMap<RegisterDto, AppUser>();

        // Mapping for Recipe and RecipeDto
        CreateMap<Recipe, RecipeDto>()
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.RecipeIngredients.Select(ri => ri.Ingredient)));
        
        CreateMap<RecipeDto, Recipe>()
    .ForMember(dest => dest.RecipeIngredients, opt => opt.MapFrom(src => src.Ingredients.Select(i => new RecipeIngredient 
    { 
        Ingredient = new Ingredient 
        { 
            Name = i.IngredientName 
        }, 
        IngredientQuantity = i.Quantity 
    })));

        CreateMap<RecipeIngredientDto, RecipeIngredient>();

        // Mapping for Ingredient and IngredientDto
        CreateMap<Ingredient, IngredientDto>();
        CreateMap<IngredientDto, Ingredient>();

        // Ensure that DateTime is mapped correctly
        CreateMap<DateTime, DateTime>().ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
        CreateMap<DateTime?, DateTime?>().ConvertUsing(d => d.HasValue ? DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : null);
    }
}

}
