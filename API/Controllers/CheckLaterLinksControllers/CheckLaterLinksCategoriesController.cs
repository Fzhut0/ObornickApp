using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.CheckLaterLinksModuleDTOS;
using API.Entities.CheckLaterLinksModuleEntities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers.CheckLaterLinksControllers
{
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
        public async Task<ActionResult> AddCategory(CheckLaterLinkCategoryDto checkLaterLinkCategoryDto)
        {
            var categoryExists = await _uow.CheckLaterLinkCategoryRepository.CategoryExists(checkLaterLinkCategoryDto.CustomName);

            if(categoryExists)
            {
                return BadRequest("Category exists");
            }

            var newCategory = new CheckLaterLinkCategory
            {
                Name = checkLaterLinkCategoryDto.CustomName
            };
            await _uow.CheckLaterLinkCategoryRepository.AddCategory(newCategory);
            if(await _uow.Complete())
            {
                return Ok("category added");
            }
            return BadRequest("something wrong with adding category");
        }

        [HttpDelete("deletecategory")]
        public async Task<ActionResult> DeleteCategory(string name)
        {
            var category = await _uow.CheckLaterLinkCategoryRepository.GetCategoryByName(name);

            if(category == null)
            {
                return BadRequest("category doesn't exist");
            }

            if(category.CategoryId == 1 || category.CategoryId == 2)
            {
                return BadRequest("can't delete this");
            }

            _uow.CheckLaterLinkCategoryRepository.DeleteCategory(category);

            if(await _uow.Complete())
            {
                return Ok();
            }

            return BadRequest("problem deleting category");
        }

        [HttpGet("getcategories")]
        public async Task<ActionResult<List<CheckLaterLinkCategoryDto>>> GetAllCategories()
        {
            var categories = await _uow.CheckLaterLinkCategoryRepository.GetAllCategories();

            if(categories.IsNullOrEmpty())
            {
                return NotFound("no categories found");
            }

            var categoryDtoList = new List<CheckLaterLinkCategoryDto>();

            foreach(var category in categories)
            {
                var categoryDto = _mapper.Map<CheckLaterLinkCategoryDto>(category);
                categoryDtoList.Add(categoryDto);
            }

            return Ok(categoryDtoList);
        }
    }
}