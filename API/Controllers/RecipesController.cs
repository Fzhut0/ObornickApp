using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class RecipesController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

        public RecipesController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        [HttpPost("addrecipe")]
        public async Task<ActionResult> AddRecipe([FromBody] RecipeDto recipeDTO)
        {
            if (recipeDTO == null || recipeDTO.Ingredients == null || !recipeDTO.Ingredients.Any())
            {
                return BadRequest("Invalid input");
            }

            if(await _uow.RecipeRepository.RecipeExists(recipeDTO.Name))
            {
                return BadRequest("recipe exists");
            }

            var newRecipe = new Recipe
            {
                Name = recipeDTO.Name,
                RecipeIngredients = new List<RecipeIngredient>()
            };

            foreach(var ing in recipeDTO.Ingredients)
            {
                var ingredient = await _uow.IngredientRepository.GetIngredientByName(ing.IngredientName);

                if(ingredient == null)
                {
                    ingredient = new Ingredient
                    {
                        Name = ing.IngredientName
                    };
                    _uow.IngredientRepository.AddIngredient(ingredient);
                    await _uow.Complete();
                }

                var recipeIngredient = new RecipeIngredient
                {
                    Recipe = newRecipe,
                    Ingredient = ingredient,
                    IngredientQuantity = ing.Quantity
                };
                newRecipe.RecipeIngredients.Add(recipeIngredient);
            }

            await _uow.RecipeRepository.AddRecipe(newRecipe);
            await _uow.Complete();

            return Ok("Recipe added successfully");
        }

        [HttpGet("getrecipe-byname")]
        public async Task<ActionResult<RecipeDto>> GetRecipeByName(string name)
        {
            var recipe = await _uow.RecipeRepository.GetRecipeByName(name);

            if(recipe == null)
            {
                return NotFound("Recipe not found");
            }

            var recipeDto = _mapper.Map<RecipeDto>(recipe);

            return Ok(recipeDto);
        }
        
        [HttpGet("getrecipe-byid")]
        public async Task<ActionResult<RecipeDto>> GetRecipeById(int id)
        {
            var recipe = await _uow.RecipeRepository.GetRecipeById(id);

            if(recipe == null)
            {
                return NotFound("Recipe not found");
            }

            var recipeDto = _mapper.Map<RecipeDto>(recipe);

            return Ok(recipeDto);
        }


    }
}
