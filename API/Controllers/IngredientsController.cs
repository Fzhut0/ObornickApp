using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class IngredientsController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public IngredientsController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        [HttpGet("getrecipeingredients")]
        public async Task<ActionResult<List<RecipeIngredientDto>>> GetRecipeIngredients(string name)
        {
            var recipe = await _uow.RecipeRepository.GetRecipeByName(name);
            
            if (recipe == null)
            {
                return NotFound("Recipe not found");
            }

            var ingredients = await _uow.IngredientRepository.GetIngredientsForRecipe(recipe.RecipeId);

            if(ingredients == null)
            {
                return NotFound("Ingredients not found");
            }

            var ingredientsDtoList = new List<RecipeIngredientDto>();

            foreach(var ingredient in ingredients)
            {
                var recipeIngredient = await _uow.IngredientRepository.GetRecipeIngredientById(ingredient.IngredientId, recipe.RecipeId);
                var ingredientDto = new RecipeIngredientDto
                {
                    IngredientName = ingredient.Name,
                    Quantity = recipeIngredient.IngredientQuantity
                };
                ingredientsDtoList.Add(ingredientDto);
            }
            return Ok(ingredientsDtoList);
        }
    }
}