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
        [HttpGet("getingredient")]
        public async Task<ActionResult<IEnumerable<RecipeDto>>> GetIngredient(int id)
        {
            var ingredient = await _uow.IngredientRepository.GetIngredientById(3);

            if (ingredient == null)
            {
                return NotFound("Ingredient not found");
            }

            var recipes = await _uow.RecipeRepository.GetRecipesByIngredientId(3);

            if (recipes == null || !recipes.Any())
            {
                return NotFound("No recipes found containing this ingredient");
            }

            var recipeDtos = _mapper.Map<IEnumerable<RecipeDto>>(recipes);

            return Ok(recipeDtos);
        }
            }
}