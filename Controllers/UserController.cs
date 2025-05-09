using Microsoft.AspNetCore.Mvc;
using AuthApi.Models;
using AuthApi.Services;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AuthApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService _userService, ITokenService _tokenService, ILogger<UserController> _logger)
        {
            this._userService = _userService;
            this._tokenService = _tokenService;
            this._logger = _logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(Register registerModel)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);
            
            if (await _userService.UserExists(registerModel.UserName)) 
                return BadRequest(new { message = "UserName already exists"});

            try
            {
                await _userService.RegisterUser(registerModel);
            } 
            catch(Exception e) 
            {
                _logger.LogError("RegisterUser method failed: {message}. Stack trace is {stacktrace}", e.Message, e.StackTrace);
                return BadRequest(new { message = "User registration failed" });
            }

            return Ok(new {message = "Sucess"});
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Login loginModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var user = await _userService.ValidateUser(loginModel.UserName, loginModel.Password);

            if (user == null)
                return Unauthorized(new { message = "Invalid credentials" });
            
            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            return Ok(new
            {
                accessToken,
                refreshToken,
                expiresIn = 300,
                user = new
                {
                    id = user.Id,
                    username = user.UserName,
                }
            });
        }
    }
}