using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.Api.DTOs;
using Talabat.Api.Extensions;
using Talabat.API.Errors;
using Talabat.Core.Entites.Identity;
using Talabat.Core.Services;

namespace Talabat.Api.Controllers
{
    
    public class AccountController : APIBaseController
    {
      

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager,
            ITokenService tokenService, IMapper mapper)
        {
            _userManager=userManager;
            _signInManager=signInManager;
            _tokenService=tokenService;
            _mapper= mapper;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return Unauthorized(new ApiResponse(401));
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));
            return Ok(new UserDto()
            {
                DisplayName = user.DiplayName,
                Email = user.Email,
                Token = await _tokenService.GetToken(user)
            });
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (CheckEmailExists(registerDto.Email).Result.Value)
                return BadRequest(new ValidationErrorResponse() { Errors = new[] { "This Email is already in use." } });
            //var user = mapper.Map<RegisterDto , AppUser>(registerDto);
            var user = new User()
            {
                DiplayName =registerDto.DisplayName,
                UserName =registerDto.Email.Split("@")[0],
                Email =registerDto.Email,
                PhoneNumber =registerDto.PhoneNumber,
                Address = new Address()
                {
                    FirstName =registerDto.FirstName,
                    SecondName =registerDto.SecondName,
                    Country =registerDto.Country,
                    City =  registerDto.City,
                    Street = registerDto.Street
                }
            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400));
            return Ok(new UserDto()
            {
                DisplayName = user.DiplayName,
                Email = user.Email,
                Token =  await _tokenService.GetToken(user)
            }); ;
        }


        [Authorize]
        [HttpGet("UserInfo")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (email == null) return NotFound(new ApiResponse(400));

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return NotFound(new ApiResponse(400));

            return (Ok(new UserDto()
            {
                DisplayName=user.DiplayName,
                Email = user.Email,
                Token= await _tokenService.GetToken(user)
            }));
        }


        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var user = await _userManager.FindWithAddressByEmailAsync(User);

            return Ok(_mapper.Map<Address, AddressDto>(user.Address));

        }



        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto newAddress)
        {
            var user = await _userManager.FindWithAddressByEmailAsync(User);

            user.Address = _mapper.Map<AddressDto, Address>(newAddress);
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest(new ValidationErrorResponse() { Errors = new[] { "An Error Occured During Updating User Address" } });

            return Ok(newAddress);

        }







        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExists([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }
    }
}
