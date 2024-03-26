using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.CheckLaterLinksModuleDTOS;
using API.Entities.CheckLaterLinksModuleEntities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.CheckLaterLinksControllers
{
    public class CheckLaterLinkController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CheckLaterLinkController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        [HttpPost("addlink")]
        public async Task<ActionResult> AddLink([FromBody] CheckLaterLinkDto laterLinkDto)
        {
            var categoryExists = await _uow.CheckLaterLinkCategoryRepository.CategoryExists(laterLinkDto.CategoryName);

            if(!categoryExists)
            {
                var newCategory = new CheckLaterLinkCategory
                {
                    Name = laterLinkDto.CategoryName
                };
                await _uow.CheckLaterLinkCategoryRepository.AddCategory(newCategory);
                await _uow.Complete();
            }

            var category = await _uow.CheckLaterLinkCategoryRepository.GetCategoryByName(laterLinkDto.CategoryName);

            var newLink = new CheckLaterLink
            {
                CustomName = laterLinkDto.CustomName,
                SavedUrl = laterLinkDto.SavedUrl,
                CategoryId = category.CategoryId
            };

            await _uow.CheckLaterLinkRepository.AddLink(newLink);

            await _uow.Complete();

            return Ok("link added");

        }
    }
}