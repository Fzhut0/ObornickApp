using API.DTOs;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
    public class UserController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public UserController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        } 

        [HttpGet("getusers")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _uow.UserRepository.GetUsersAsync();

            var userDtoList = new List<UserDto>();

            foreach(var user in users)
            {
                var userDto = _mapper.Map<UserDto>(user);
                userDtoList.Add(userDto);
            }

            return Ok(users);
        }

        [HttpGet("hasusermessageserviceid")]
        public async Task<ActionResult<bool>> HasMessagingServiceId()
        {
            var user = await _uow.UserRepository.GetUserByUsernameAsync(User.Identity.Name);

            if(user == null)
            {
                return BadRequest("Brak użytkownika");
            }

            if (!user.MessageServiceRecipientId.IsNullOrEmpty())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}