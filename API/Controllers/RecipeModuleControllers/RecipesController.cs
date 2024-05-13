using API.DTOs;
using API.DTOs.RecipeModuleDTOS;
using API.Entities;
using API.Entities.RecipeModuleEntities;
using API.Interfaces;
using AutoMapper;
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


        [HttpPost("addrecipe")]
        public async Task<ActionResult> AddRecipe([FromBody] RecipeDto recipeDTO)
        {
            var user = await _uow.UserRepository.GetUserByUsernameAsync(User.Identity.Name);
            var userId = user.Id;

            if(user == null)
            {
                return BadRequest("Brak użytkownika");
            }

            if (recipeDTO == null || recipeDTO.Ingredients == null || !recipeDTO.Ingredients.Any() 
            || recipeDTO.Name.IsNullOrEmpty() || recipeDTO.RecipeDescriptionSteps.IsNullOrEmpty())
            {
                return BadRequest("Brak składników lub kroków wykonania przepisu.");
            }

            if(await _uow.RecipeRepository.RecipeExists(recipeDTO.Name))
            {
                return BadRequest("Przepis o tej nazwie już istnieje");
            }

            var newRecipe = new Recipe
            {
                Name = recipeDTO.Name,
                RecipeIngredients = new List<RecipeIngredient>(),
                UserId = userId,
                RecipeDescriptionSteps = new List<RecipeDescriptionStep>()
            };

            foreach(var step in recipeDTO.RecipeDescriptionSteps)
            {
                    var descriptionStep = _mapper.Map<RecipeDescriptionStep>(step);
                newRecipe.RecipeDescriptionSteps.Add(descriptionStep);
            }


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

            var user = await _uow.UserRepository.GetUserByUsernameAsync(User.Identity.Name);
            var userId = user.Id;

            if(user == null)
            {
                return BadRequest("Brak użytkownika");
            }

            var recipe = await _uow.RecipeRepository.GetRecipeByName(name, userId);

            if(recipe == null)
            {
                return NotFound("Brak przepisu");
            }

            var recipeDto = _mapper.Map<RecipeDto>(recipe);

            return Ok(recipeDto);
        }
    
        [HttpGet("getrecipe-byid")]
        public async Task<ActionResult<RecipeDto>> GetRecipeById(int id)
        {
            var user = await _uow.UserRepository.GetUserByUsernameAsync(User.Identity.Name);

            if(user == null)
            {
                return BadRequest("Brak użytkownika");
            }

            var recipe = await _uow.RecipeRepository.GetRecipeById(id);

            if(recipe == null)
            {
                return NotFound("Brak przepisu");
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

        [HttpDelete("deleterecipe")]
        public async Task<ActionResult> DeleteRecipe(string name)
        {

            var user = await _uow.UserRepository.GetUserByUsernameAsync(User.Identity.Name);
            var userId = user.Id;

            if(user == null)
            {
                return BadRequest("Brak użytkownika");
            }

            var recipe = await _uow.RecipeRepository.GetRecipeByName(name, userId);

            if(recipe == null)
            {
                return BadRequest("Brak przepisu");
            }

            _uow.RecipeRepository.DeleteRecipe(recipe);

            if(await _uow.Complete())
            {
                return Ok();
            }

            return BadRequest("Błąd usuwania przepisu");
        }

        [HttpPut("updaterecipe")]
        public async Task<ActionResult> UpdateRecipe([FromBody] RecipeDto recipeDTO)
        {

            var user = await _uow.UserRepository.GetUserByUsernameAsync(User.Identity.Name);
            var userId = user.Id;

            if(user == null)
            {
                return BadRequest("Brak użytkownika");
            }

            if (recipeDTO == null || recipeDTO.Ingredients.IsNullOrEmpty() 
            || recipeDTO.Name.IsNullOrEmpty() || recipeDTO.RecipeDescriptionSteps.IsNullOrEmpty())
            {
                return BadRequest("Nieprawidłowe dane");
            }

            var existingRecipe = await _uow.RecipeRepository.GetRecipeByName(recipeDTO.OriginalName, userId);

            if (existingRecipe == null)
            {
                return BadRequest("Recipe not found");
            }

            existingRecipe.RecipeIngredients.Clear();
            existingRecipe.RecipeDescriptionSteps.Clear();

            existingRecipe.Name = recipeDTO.Name;
            foreach(var step in recipeDTO.RecipeDescriptionSteps)
            {
                var descriptionStep = _mapper.Map<RecipeDescriptionStep>(step);
                existingRecipe.RecipeDescriptionSteps.Add(descriptionStep);
            }

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

            return BadRequest("Bład aktualizacji przepisu");
        }

        
        [HttpGet("userhasrecipe")]
        public async Task<ActionResult<bool>> GetRecipeBelongsToUser(string recipeName)
        {
            var user = await _uow.UserRepository.GetUserByUsernameAsync(User.Identity.Name);
            var userId = user.Id;

            if(user == null)
            {
                return BadRequest("Brak użytkownika");
            }

            var recipe = await _uow.RecipeRepository.GetRecipeByName(recipeName, userId);

            if(recipe != null)
            {
                return true;
            }
            return false;
        }

        [HttpGet("getrecipedescriptionsteps")]
        public async Task<ActionResult<List<RecipeDescriptionStepDto>>> GetRecipeDescriptionSteps(int recipeId)
        {
            var user = await _uow.UserRepository.GetUserByUsernameAsync(User.Identity.Name);
            var userId = user.Id;

            if(user == null)
            {
                return BadRequest("Brak użytkownika");
            }

            var recipe = await _uow.RecipeRepository.GetRecipeById(recipeId);

            if (recipe == null)
            {
                return NotFound("Recipe not found");
            }

            var recipeDescriptionSteps = await _uow.RecipeRepository.GetRecipeDescriptionSteps(recipeId);

            if(recipeDescriptionSteps.IsNullOrEmpty())
            {
                return BadRequest("Brak kroków w przepisie");
            }

            var recipeDescriptionDtoList = new List<RecipeDescriptionStepDto>();

            foreach(var step in recipeDescriptionSteps)
            {
                var recipeDescriptionDto = _mapper.Map<RecipeDescriptionStepDto>(step);

                recipeDescriptionDtoList.Add(recipeDescriptionDto);
            }
            return Ok(recipeDescriptionDtoList);
        }
    }

}

