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

            try
            {
                // TODO : generate email verification token
                // Generate email verification token
                //var token = await _userService.GenerateEmailConfirmationToken(registerModel.Email);
                //Send verification email
                //await _emailService.SendVerificationEmail(registerModel.Email, token);
            }
            catch(Exception e)
            {
                _logger.LogError("Verification email failed: {message}. Stack trace is {stacktrace}", e.Message, e.StackTrace);
                return BadRequest(new { message = "Email verification sending failed" });
            }

            return Ok(new {message = "Sucess"});
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Login loginModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var user = await _userService.ValidateUser(loginModel.Email, loginModel.Password);

            if (user == null)
                return Unauthorized(new { message = "Invalid credentials or email unverified" });
            
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
                    email = user.Email,
                    firstName = user.FirstName,
                    lastName = user.LastName
                }
            });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPassword forgotPasswordModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userService.GetUserByEmail(forgotPasswordModel.Email);

            // return Ok for security reason, not revealing the user does or does not exist
            if (user == null)
                return Ok();
            
            var token = _tokenService.GeneratePasswordResetToken(user);

            // TODO: mail
            // await _emailService.SendPasswordResetEmail(forgotPasswordModel.Email, token);

            return Ok(); 
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPassword resetPasswordModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await _userService.ResetPassword(resetPasswordModel);

            if (!result)
                return BadRequest(new { message = "Password reset failed" });
            
            return Ok();
        }

    }
}