using API.DTOs;
using API.Entities;
using API.Entities.CheckLaterLinksModuleEntities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController : BaseApiController
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uow;

    public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, IMapper mapper, IUnitOfWork uow)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _mapper = mapper;
        _uow = uow;
    }

    [HttpPost("register")] // POST: api/account/register
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if (await UserExists(registerDto.Username))
        {
            return BadRequest("Nazwa użytkownika zajęta");
        }

        var user = _mapper.Map<AppUser>(registerDto);
        var result = await _userManager.CreateAsync(user, registerDto.Password);
        

        if(!result.Succeeded)
        {
            return BadRequest("Błąd rejestracji");
        }

        var rolesResults = await _userManager.AddToRoleAsync(user, "Member");

        if(!rolesResults.Succeeded)
        {
            return BadRequest("Błąd rejestracji");
        }

        var newCategory = new CheckLaterLinkCategory
            {
                Name = "Bez kategorii",
                UserId = user.Id
            };

        await _uow.CheckLaterLinkCategoryRepository.AddCategory(newCategory);
        user.Categories.Add(newCategory);

        await _uow.Complete();

        return new UserDto
        {
            Username = user.UserName,
            Token = await _tokenService.CreateToken(user),
        };
    }


    [HttpPost("login")]

    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await _userManager.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.Username);

         if(user == null)
         {
            return Unauthorized("Nieprawidłowa nazwa użytkownika lub hasło");
         }

        var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

        if(!result)
        {
            return Unauthorized("Nieprawidłowa nazwa użytkownika lub hasło");
        }

        return new UserDto
        {
            Username = user.UserName,
            Token = await _tokenService.CreateToken(user),
        };
    }

    private async Task<bool> UserExists(string username)
    {
        return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
    }
}
