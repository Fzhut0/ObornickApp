using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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

    [Authorize(Roles = "Admin")]
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
                await _uow.IngredientRepository.AddIngredient(ingredient);
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

    [HttpGet("getrecipes")]
    public async Task<ActionResult<List<RecipeDto>>> GetAllRecipes()
    {
        var recipes = await _uow.RecipeRepository.GetAllRecipes();

        if(recipes.IsNullOrEmpty())
        {
            return NotFound("No recipes found");
        }

        var recipeDtoList = new List<RecipeDto>();

        foreach(var recipe in recipes)

        {
            var recipeDto = _mapper.Map<RecipeDto>(recipe);
            recipeDtoList.Add(recipeDto);
        }

        return Ok(recipeDtoList);
    }

    [Authorize(Policy = "RequireAdminRole")]
    [HttpDelete("deleterecipe")]
    public async Task<ActionResult> DeleteRecipe(string name)
    {
        var recipe = await _uow.RecipeRepository.GetRecipeByName(name);

        if(recipe == null)
        {
            return BadRequest("recipe doesn't exist");
        }

        _uow.RecipeRepository.DeleteRecipe(recipe);

        if(await _uow.Complete())
        {
            return Ok();
        }

        return BadRequest("problem deleting recipe");
    }

    [HttpPut("updaterecipe")]
    public async Task<ActionResult> UpdateRecipe([FromBody] RecipeDto recipeDTO)
    {
        if (recipeDTO == null || recipeDTO.Ingredients == null)
        {
            return BadRequest("Invalid input");
        }

        var existingRecipe = await _uow.RecipeRepository.GetRecipeByName(recipeDTO.OriginalName);

        if (existingRecipe == null)
        {
            return BadRequest("Recipe not found");
        }

        existingRecipe.RecipeIngredients.Clear();
        existingRecipe.Name = recipeDTO.Name;


        foreach (var ing in recipeDTO.Ingredients)
        {
            var ingredient = await _uow.IngredientRepository.GetIngredientByName(ing.IngredientName);

            if (ingredient == null)
            {
                ingredient = new Ingredient
                {
                    Name = ing.IngredientName
                };
                await _uow.IngredientRepository.AddIngredient(ingredient);
            }


            var recipeIngredient = new RecipeIngredient
            {
                Recipe = existingRecipe,
                IngredientId = ingredient.IngredientId,
                Ingredient = ingredient,
                IngredientQuantity = ing.Quantity
            };
            existingRecipe.RecipeIngredients.Add(recipeIngredient);                  
        }
        if (await _uow.Complete())
        {
            return Ok("Recipe updated");
        }

        return BadRequest("Problem updating recipe");
        }

    }
}

