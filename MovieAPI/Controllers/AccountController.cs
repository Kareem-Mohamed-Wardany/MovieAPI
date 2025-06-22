using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieAPI.Dtos;
using MovieAPI.Errors;
using MovieAPI.Services.Auth;
using Swashbuckle.AspNetCore.Annotations;

namespace MovieAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAuthService _authService;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager,
            IAuthService authService,
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _authService = authService;
            _signInManager = signInManager;
        }
        [SwaggerOperation(
            Summary = "Login to the system with vaild Email and Password",
            Description = "Login with vaild Email and Password you will get JWT Token to access most of system features"
        )]
        [HttpPost("login")]
        [ProducesResponseType(typeof(UserDto), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return Unauthorized(new ApiResponse(401, "Invalid Login"));

            var res = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (!res.Succeeded) return Unauthorized(new ApiResponse(401, "Invalid Login"));
            return Ok(new UserDto()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Token = await _authService.CreateTokenAsync(user, _userManager),
                Roles= await _userManager.GetRolesAsync(user)
            });

        }
        [SwaggerOperation(
            Summary = "Register to the system with vaild Data",
            Description = "Register with vaild Data"
        )]
        [HttpPost("register")]
        [ProducesResponseType(201)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), 400)]
        public async Task<IActionResult> Register(RegisterDto model)
        {

            var user = new IdentityUser()
            {
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.Phone
            };
            var res = await _userManager.CreateAsync(user, model.Password);
            if (!res.Succeeded) return BadRequest(new ApiValidationErrorResponse() { Errors = res.Errors.Select(e => e.Description) });
            await _userManager.AddToRoleAsync(user, "User");
            return Ok(new ApiResponse(201, "User Created Successfully!"));

        }
    }
}
