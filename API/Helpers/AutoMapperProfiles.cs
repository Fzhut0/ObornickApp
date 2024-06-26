using API.DTOs;
using API.DTOs.CheckLaterLinksModuleDTOS;
using API.DTOs.RecipeModuleDTOS;
using API.Entities;
using API.Entities.CheckLaterLinksModuleEntities;
using API.Entities.RecipeModuleEntities;
using AutoMapper;

namespace API.Helpers
{    public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<AppUser, UserDto>();
        CreateMap<RegisterDto, AppUser>();

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
        })))
        .ForMember(dest => dest.RecipeDescriptionSteps, opt => opt.MapFrom(src => src.RecipeDescriptionSteps.Select(rds => new RecipeDescriptionStep 
            { 
                Description = rds.Description 
            })));

        CreateMap<RecipeIngredientDto, RecipeIngredient>();
        CreateMap<RecipeIngredient, RecipeIngredientDto>();

        CreateMap<CheckLaterLinkDto, CheckLaterLink>();
        CreateMap<CheckLaterLink, CheckLaterLinkDto>();

        CreateMap<CheckLaterLinkCategory, CheckLaterLinkCategoryDto>()
                .ForMember(dest => dest.CustomName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Links, opt => opt.MapFrom(src => src.CheckLaterLinks));

        CreateMap<Ingredient, IngredientDto>();
        CreateMap<Ingredient, RecipeIngredientDto>();
        CreateMap<IngredientDto, Ingredient>();

        CreateMap<DateTime, DateTime>().ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
        CreateMap<DateTime?, DateTime?>().ConvertUsing(d => d.HasValue ? DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : null);

        CreateMap<RecipeDescriptionStep, RecipeDescriptionStepDto>();
        CreateMap<RecipeDescriptionStepDto, RecipeDescriptionStep>();
    }
}

}
