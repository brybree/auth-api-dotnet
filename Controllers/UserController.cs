using Microsoft.AspNetCore.Mvc;
using AuthApi.Models;
using AuthApi.Services;

namespace AuthApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService _userService, ITokenService _tokenService, IEmailService _emailService, ILogger<UserController> _logger)
        {
            this._userService = _userService;
            this._tokenService = _tokenService;
            this._emailService = _emailService;
            this._logger = _logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(Register registerModel)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);
            
            if (await _userService.UserExists(registerModel.Email)) 
                return BadRequest(new { message = "Email already exists"});

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
    }
}