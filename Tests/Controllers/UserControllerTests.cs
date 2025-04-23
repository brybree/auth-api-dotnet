using AuthApi.Controllers;
using AuthApi.Models;
using AuthApi.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace AuthApi.Tests.Controllers
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<ITokenService> _mockTokenService;
        private readonly Mock<IEmailService> _mockEmailService;
        private readonly UserController _userController;
        private readonly Mock<ILogger<UserController>> _mockLogger;

        public UserControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _mockTokenService = new Mock<ITokenService>();
            _mockEmailService = new Mock<IEmailService>();
            _mockLogger = new Mock<ILogger<UserController>>();
            _userController = new UserController(_mockUserService.Object, _mockTokenService.Object, _mockEmailService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Register_Valid_User()
        {
            // Given
            var registerModel = new Register
            {
                Email = "test@test.com",
                Password = "StrongPassword123!",
                ConfirmPassword = "StrongPassword123!",
            };
            // user should not already exist
            _mockUserService
                .Setup(x => x.UserExists(It.IsAny<string>()))
                .Returns(Task.FromResult(false));

            // user should have been register
            _mockUserService
                .Setup(x => x.RegisterUser(It.IsAny<Register>()))
                .Returns(Task.FromResult(true));

            // verification mail should have been sent
            _mockEmailService
                .Setup(x => x.SendVerificationEmail(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // When
            var result = await _userController.Register(registerModel);

            // Then
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Register_Existing_User()
        {
            // Given
            var registerModel = new Register
            {
                Email = "test@test.com",
                Password = "StrongPassword123!",
                ConfirmPassword = "StrongPassword123!",
            };

            // user should already exist
            _mockUserService
                .Setup(x => x.UserExists(It.IsAny<string>()))
                .Returns(Task.FromResult(true));

            // When
            var result = await _userController.Register(registerModel);

            // Then
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Login_Valid_Credentials()
        {
            // Given
            var loginModel = new Login
            {
                Email = "test@example.com",
                Password = "Password123!"
            };
            var user = new User { Id = Guid.NewGuid().ToString(), Email = "test@example.com", EmailConfirmed = true };
            
            // User should exist and confirmed
            _mockUserService.Setup(x => x.ValidateUser(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(user);
            // An access token must be generated 
            _mockTokenService.Setup(x => x.GenerateAccessToken(It.IsAny<User>())).Returns("valid_token");
            // A refresh token must be generated
            _mockTokenService.Setup(x => x.GenerateRefreshToken()).Returns("refresh_token");
            
            // When
            var result = await _userController.Login(loginModel);
            
            // Then
            var okResult = Assert.IsType<OkObjectResult>(result);
            dynamic tokenResult = okResult.Value;
            Assert.NotNull(tokenResult);
        }
    }
}