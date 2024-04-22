using API.DTOs.CheckLaterLinksModuleDTOS;
using API.Entities.CheckLaterLinksModuleEntities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
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
            var user = await _uow.UserRepository.GetUserByUsernameAsync(User.Identity.Name);

            if(user == null)
            {
                return BadRequest("no user");
            }

            var userId = user.Id;

            var categoryExists = await _uow.CheckLaterLinkCategoryRepository.CategoryExists(laterLinkDto.CategoryId, userId);

            if(!categoryExists)
            {
                return BadRequest("Category doesn't exist");
            }

            if(!HttpExtensions.CheckLinkValidity(laterLinkDto.SavedUrl))
            {
                return BadRequest("link is incorrect");
            }

            // var existingLinkName = await _uow.CheckLaterLinkRepository.GetCheckLaterLinkByName(laterLinkDto.CustomName, userId);
            // var existingLinkUrl = await _uow.CheckLaterLinkRepository.GetCheckLaterLinkByUrl(laterLinkDto.SavedUrl, userId);

            // if(existingLinkName != null || existingLinkUrl != null)
            // {
                
            //     return BadRequest("Link is already added with name:" + existingLinkName.CustomName);
            // }

            var category = await _uow.CheckLaterLinkCategoryRepository.GetCategoryById(laterLinkDto.CategoryId, userId);
            

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
            var user = await _uow.UserRepository.GetUserByUsernameAsync(User.Identity.Name);

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

            var viewedCategory = await _uow.CheckLaterLinkCategoryRepository.GetCategoryById(defaultCategory.CategoryId, userId);

            if(defaultCategory == viewedCategory)
            {
                return BadRequest("you can't move to the same category");
            }

            link.CategoryId = viewedCategory.CategoryId;

            if(await _uow.Complete())
            {
                return Ok();
            }

            return BadRequest("something wrong with updating link");

        }

        [HttpDelete("deletelink")]
        public async Task<ActionResult> DeleteLink(string name)
        {
            var user = await _uow.UserRepository.GetUserByUsernameAsync(User.Identity.Name);
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

        [HttpPut("updatelinkcategory")]
        public async Task<ActionResult> UpdateLinkCategory([FromBody]CheckLaterLinkDto checkLaterLinkDto)
        {
            var user = await _uow.UserRepository.GetUserByUsernameAsync(User.Identity.Name);
            var userId = user.Id;

            if(user == null)
            {
                return BadRequest("no user");
            }

            if(checkLaterLinkDto == null)
            {
                return BadRequest("invalid input");
            }

            var existingLink = await _uow.CheckLaterLinkRepository.GetCheckLaterLinkByName(checkLaterLinkDto.CustomName, userId);
            var category = await _uow.CheckLaterLinkCategoryRepository.GetCategoryById(checkLaterLinkDto.CategoryId, userId);

            existingLink.CategoryId = category.CategoryId;

            if(await _uow.Complete())
            {
                return Ok("link category updated");
            }

            return BadRequest("problem updating link");
        }
    }
}