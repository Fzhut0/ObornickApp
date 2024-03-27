using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.CheckLaterLinksModuleDTOS;
using API.Entities.CheckLaterLinksModuleEntities;
using API.Extensions;
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
                return BadRequest("Category doesn't exist");
            }

            if(!HttpExtensions.CheckLinkValidity(laterLinkDto.SavedUrl))
            {
                return BadRequest("link is incorrect");
            }

            var existingLinkName = await _uow.CheckLaterLinkRepository.GetCheckLaterLinkByName(laterLinkDto.CustomName);
            var existingLinkUrl = await _uow.CheckLaterLinkRepository.GetCheckLaterLinkByUrl(laterLinkDto.SavedUrl);

            if(existingLinkName != null || existingLinkUrl != null)
            {
                
                return BadRequest("Link is already added with name:" + existingLinkName.CustomName);
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