using API.DTOs;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{

    public class AdminController : BaseApiController
    {
        private readonly IUnitOfWork _uow;

        public AdminController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("addfacebookid")]
        public async Task<IActionResult> AddFacebookUserId([FromBody]UserDto userDto)
        {

            var user = await _uow.UserRepository.GetUserByUsernameAsync(userDto.Username);
            var userId = user.Id;

            if(user == null)
            {
                return BadRequest("Brak użytkownika");
            }

            if(!user.MessageServiceRecipientId.IsNullOrEmpty())
            {
                return BadRequest("Id exists");
            }

            user.MessageServiceRecipientId = userDto.MessageServiceRecipientId;

            if (await _uow.Complete())
            {
                return Ok("Facebook id added");
            }

            return BadRequest("Problem adding id");
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPut("clearfacebookid")]
        public async Task<IActionResult> DeleteUserFacebookId([FromBody]UserDto userDto)
        {
            var user = await _uow.UserRepository.GetUserByUsernameAsync(userDto.Username);

            if(user == null)
            {
                return BadRequest("Brak użytkownika");
            }

            if(user.MessageServiceRecipientId.IsNullOrEmpty())
            {
                return BadRequest("Id is null");
            }

            user.MessageServiceRecipientId = "";

            if (await _uow.Complete())
            {
                return Ok("Facebook id added");
            }

            return BadRequest("Problem adding id");
        }
    }
}