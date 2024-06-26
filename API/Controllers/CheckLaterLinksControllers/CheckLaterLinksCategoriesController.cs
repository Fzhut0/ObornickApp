using API.DTOs.CheckLaterLinksModuleDTOS;
using API.Entities.CheckLaterLinksModuleEntities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers.CheckLaterLinksControllers
{
    [Authorize]
    public class CheckLaterLinksCategoriesController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CheckLaterLinksCategoriesController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        [HttpPost("addcategory")]
        public async Task<ActionResult> AddCategory([FromBody]CheckLaterLinkCategoryDto checkLaterLinkCategoryDto)
        {
            var user = await _uow.UserRepository.GetUserByUsernameAsync(User.Identity.Name);
            var userId = user.Id;

            if(user == null)
            {
                return BadRequest("no user");
            }

            if(checkLaterLinkCategoryDto.CustomName.IsNullOrEmpty())
            {
                return BadRequest("Brak nazwy kategorii");
            }

            var newCategory = new CheckLaterLinkCategory
            {
                Name = checkLaterLinkCategoryDto.CustomName,
                UserId = userId
            };

            await _uow.CheckLaterLinkCategoryRepository.AddCategory(newCategory);
            user.Categories.Add(newCategory);

            if(await _uow.Complete())
            {
                return Ok("category added");
            }
            return BadRequest("something wrong with adding category");
        }

        [HttpPost("addsubcategory")]
        public async Task<ActionResult> AddSubcategory([FromBody]CheckLaterLinkCategoryDto checkLaterLinkCategoryDto)
        {
            var user = await _uow.UserRepository.GetUserByUsernameAsync(User.Identity.Name);
            var userId = user.Id;

            if(user == null)
            {
                return BadRequest("Nieznany użytkownik");
            }

            if(checkLaterLinkCategoryDto.CustomName.IsNullOrEmpty())
            {
                return BadRequest("Brak nazwy kategorii");
            }

            var parentCategory = await _uow.CheckLaterLinkCategoryRepository.GetCategoryById(checkLaterLinkCategoryDto.CategoryId, userId);

            var newCategory = new CheckLaterLinkCategory
            {
                Name = checkLaterLinkCategoryDto.CustomName,
                UserId = userId
            };

            await _uow.CheckLaterLinkCategoryRepository.AddSubcategory(newCategory, parentCategory.CategoryId);

            if(await _uow.Complete())
            {
                return Ok("Podkategoria dodana");
            }
            return BadRequest("Coś poszło nie tak");
        }

        [HttpDelete("deletecategory")]
        public async Task<ActionResult> DeleteCategory(int categoryId)
        {
            var user = await _uow.UserRepository.GetUserByUsernameAsync(User.Identity.Name);
            var userId = user.Id;

            if(user == null)
            {
                return BadRequest("Brak użytkownika");
            }

            var category = await _uow.CheckLaterLinkCategoryRepository.GetCategoryById(categoryId, userId);

            if(category == null)
            {
                return BadRequest("Kategoria nie istnieje.");
            }

            if(category.Name == "Bez kategorii")
            {
                return BadRequest("Nie możesz tego usunąć.");
            }

            if(!category.Subcategories.IsNullOrEmpty())
            {
                return BadRequest("Nie można usunąć ponieważ kategoria zawiera podkategorie. Najpierw usuń wszystkie podkategorie.");
            }

            if(category.CheckLaterLinks.Count > 0)
            {
                var defaultCategory = await _uow.CheckLaterLinkCategoryRepository.GetUserDefaultCategory(userId);

                foreach(var link in category.CheckLaterLinks)
                {
                    link.CategoryId = defaultCategory.CategoryId;
                    defaultCategory.CheckLaterLinks.Add(link);
                }
            }

            _uow.CheckLaterLinkCategoryRepository.DeleteCategory(category);

            if(await _uow.Complete())
            {
                return Ok();
            }

            return BadRequest("Coś poszło nie tak");
        }

        [HttpGet("getcategories")]
        public async Task<ActionResult<List<CheckLaterLinkCategoryDto>>> GetAllCategories()
        {

            var user = await _uow.UserRepository.GetUserByUsernameAsync(User.Identity.Name);

            if(user == null)
            {
                return BadRequest("no user");
            }

            var userId = user.Id;

            var categories = await _uow.CheckLaterLinkCategoryRepository.GetAllCategories(userId);

            if(categories.IsNullOrEmpty())
            {
                return NotFound("Nie znaleziono kategorii");
            }

            var categoryDtoList = new List<CheckLaterLinkCategoryDto>();

            foreach(var category in categories)
            {
                if(category.ParentCategoryId.HasValue)
                {
                    continue;
                }
                var categoryDto = _mapper.Map<CheckLaterLinkCategoryDto>(category);
                categoryDtoList.Add(categoryDto);
            }

            return Ok(categoryDtoList);
        }

        [HttpGet("getsubcategories")]
        public async Task<ActionResult<List<CheckLaterLinkCategoryDto>>> GetSubcategories(int parentCategoryId)
        {
            var user = await _uow.UserRepository.GetUserByUsernameAsync(User.Identity.Name);

            if(user == null)
            {
                return BadRequest("no user");
            }

            var userId = user.Id;

            var subcategories = await _uow.CheckLaterLinkCategoryRepository.GetSubcategories(parentCategoryId, userId);

            var categoryDtoList = new List<CheckLaterLinkCategoryDto>();

            foreach(var category in subcategories)
            {
                var categoryDto = _mapper.Map<CheckLaterLinkCategoryDto>(category);
                categoryDto.IsSubcategory = true;
                categoryDtoList.Add(categoryDto);
            }
            return Ok(categoryDtoList);
        }

        [HttpGet("getcategorynamebyid")]
        public async Task<ActionResult<CheckLaterLinkCategoryDto>> GetCategoryNameById(int categoryId)
        {
            var user = await _uow.UserRepository.GetUserByUsernameAsync(User.Identity.Name);

            if(user == null)
            {
                return BadRequest("no user");
            }

            var userId = user.Id;
            var category = await _uow.CheckLaterLinkCategoryRepository.GetCategoryById(categoryId, userId);

            if(category == null)

            {
                return BadRequest();
            }
            var categoryDto = _mapper.Map<CheckLaterLinkCategoryDto>(category);
            if(categoryDto.ParentCategoryName != null)
            {
                categoryDto.IsSubcategory = true;
            }
            return Ok(categoryDto);
        }
    }
}