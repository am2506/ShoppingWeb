using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RouteWebAPI.Dtos;
using RouteWebAPI.Dtos.OrderDto;
using RouteWebAPI.Helpers.HandleErrors;
using System.Security.Claims;
using Shopping.Core.IdentityModels;
using Shopping.Core.IServices;
using Shopping.Service.AuthService;

namespace RouteWebAPI.Controllers
{
    public class AccountController : BaseAPIController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IAuthService _auth;
        private readonly IMapper _mapper;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,IConfiguration configuration , IAuthService auth,IMapper mapper)
        {
            _userManager = userManager;
           _signInManager = signInManager;
           _configuration = configuration;
           _auth = auth;
          _mapper = mapper;
        }
        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto model)
        {
            var UserExcited = await _userManager.FindByEmailAsync(model.Email);
            if (UserExcited is null)
                return Unauthorized(new APIErrorResponse(StatusCodes.Status401Unauthorized,"User Not Register"));
            var Result = await _signInManager.CheckPasswordSignInAsync(UserExcited, model.Password, false);
            if (!Result.Succeeded)
            {
                return Unauthorized(new APIErrorResponse(StatusCodes.Status401Unauthorized, "User Not Register"));
            }
            return Ok(new UserDto()
            {
                DisplayName = UserExcited.DisplayName,
                Email = UserExcited.Email,
                Token = await _auth.CreateTokenAsync(UserExcited)
            });
        }
        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto model)
        {
            
            var UserExcited = await _userManager.FindByEmailAsync(model.Email);
            if (UserExcited is not null)
                return BadRequest(new APIErrorResponse(StatusCodes.Status400BadRequest, "This Email Already Registered"));
            var User = new ApplicationUser()
            {
                DisplayName = model.FullName,
                UserName = model.Email.Split('@')[0],
                Email = model.Email,
            };
            var newUser = await _userManager.CreateAsync(User, model.password);
            if (!newUser.Succeeded)
                return BadRequest(new APIErrorResponse(StatusCodes.Status400BadRequest, "An Error Occured During Register"));
           
            return Ok(new UserDto()
            {
                DisplayName = User.DisplayName,
                Email = User.Email,
                Token = await _auth.CreateTokenAsync(User)
            });
        }
        [HttpPut("Address")]
        public async Task<ActionResult<AddressDto>> UpdateAddress(AddressDto UpdatedAddress)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return Unauthorized();
            var address = _mapper.Map<AddressDto, Address>(UpdatedAddress);
           // address.Id = user.Address.Id??;
            user.Address = address;
            var Result = await _userManager.UpdateAsync(user);
            if (!Result.Succeeded) return BadRequest(Result);
            return Ok(UpdatedAddress);
        }
    }
}
