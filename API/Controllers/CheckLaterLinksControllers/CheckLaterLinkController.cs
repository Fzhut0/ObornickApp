using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.CheckLaterLinksModuleDTOS;
using API.Entities.CheckLaterLinksModuleEntities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.CheckLaterLinksControllers
{
    [Authorize]
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
            var user = await _uow.UserRepository.GetUserByUsernameAsync(laterLinkDto.Username);

            if(user == null)
            {
                return BadRequest("no user");
            }

            var userId = user.Id;

            var categoryExists = await _uow.CheckLaterLinkCategoryRepository.CategoryExists(laterLinkDto.CategoryName, userId);

            if(!categoryExists)
            {
                return BadRequest("Category doesn't exist");
            }

            if(!HttpExtensions.CheckLinkValidity(laterLinkDto.SavedUrl))
            {
                return BadRequest("link is incorrect");
            }

            var existingLinkName = await _uow.CheckLaterLinkRepository.GetCheckLaterLinkByName(laterLinkDto.CustomName, userId);
            var existingLinkUrl = await _uow.CheckLaterLinkRepository.GetCheckLaterLinkByUrl(laterLinkDto.SavedUrl, userId);

            if(existingLinkName != null || existingLinkUrl != null)
            {
                
                return BadRequest("Link is already added with name:" + existingLinkName.CustomName);
            }

            var category = await _uow.CheckLaterLinkCategoryRepository.GetCategoryByName(laterLinkDto.CategoryName, userId);
            

            var newLink = new CheckLaterLink
            {
                CustomName = laterLinkDto.CustomName,
                SavedUrl = laterLinkDto.SavedUrl,
                CategoryId = category.CategoryId,
                UserId = userId
            };

            await _uow.CheckLaterLinkRepository.AddLink(newLink);

            await _uow.Complete();

            return Ok("link added");
        }

        [HttpPut("setlinkviewed")]
        public async Task<ActionResult> SetLinkAsViewed([FromBody] CheckLaterLinkDto checkLaterLinkDto)
        {
            var user = await _uow.UserRepository.GetUserByUsernameAsync(checkLaterLinkDto.Username);

            if(user == null)
            {
                return BadRequest("no user");
            }

            var userId = user.Id;

            var link = await _uow.CheckLaterLinkRepository.GetCheckLaterLinkByName(checkLaterLinkDto.CustomName, userId);

            if(link == null)
            {
                return BadRequest("link doesn't exist");
            }

            var defaultCategory = await _uow.CheckLaterLinkCategoryRepository.GetUserDefaultCategory(userId);

            var viewedCategory = await _uow.CheckLaterLinkCategoryRepository.GetCategoryById(defaultCategory.CategoryId, userId); // id = 2 is viewed category, id = 1 is default category

            link.CategoryId = viewedCategory.CategoryId;

            if(await _uow.Complete())
            {
                return Ok();
            }

            return BadRequest("something wrong with updating link");

        }

        [HttpDelete("deletelink")]
        public async Task<ActionResult> DeleteLink(string name, string username)
        {
            var user = await _uow.UserRepository.GetUserByUsernameAsync(username);
            var userId = user.Id;

            if(user == null)
            {
                return BadRequest("no user");
            }

            var link = await _uow.CheckLaterLinkRepository.GetCheckLaterLinkByName(name, userId);

            if(link == null)
            {
                return BadRequest("link doesn't exist");
            }

            _uow.CheckLaterLinkRepository.DeleteLink(link);

            if(await _uow.Complete())
            {
                return Ok();
            }

            return BadRequest("link wasn't deleted");
        }
    }
}