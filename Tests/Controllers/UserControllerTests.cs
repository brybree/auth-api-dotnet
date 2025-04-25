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
        private readonly UserController _userController;
        private readonly Mock<ILogger<UserController>> _mockLogger;

        public UserControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _mockTokenService = new Mock<ITokenService>();
            _mockLogger = new Mock<ILogger<UserController>>();
            _userController = new UserController(_mockUserService.Object, _mockTokenService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Register_Valid_User()
        {
            // Given
            var registerModel = new Register
            {
                UserName = "username",
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
                UserName = "username",
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
                UserName = "username",
                Password = "Password123!"
            };
            var user = new User { Id = Guid.NewGuid().ToString(), UserName = "username" };
            
            // User should exist and confirmed
            _mockUserService
                .Setup(x => x.ValidateUser(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(user);
            // An access token must be generated 
            _mockTokenService
                .Setup(x => x.GenerateAccessToken(It.IsAny<User>()))
                .Returns("valid_token");
            // A refresh token must be generated
            _mockTokenService
                .Setup(x => x.GenerateRefreshToken())
                .Returns("refresh_token");
            
            // When
            var result = await _userController.Login(loginModel);
            
            // Then
            var okResult = Assert.IsType<OkObjectResult>(result);
            dynamic tokenResult = okResult.Value;
            Assert.NotNull(tokenResult);
        }

        [Fact]
        public async Task Login_Invalid_Credentials()
        {
            // Given
            var loginModel = new Login
            {
                UserName = "username",
                Password = "Password123!"
            };
            var user = new User { Id = Guid.NewGuid().ToString(), UserName = "username" };

            // Either user does not exist or credentials are invalid
            _mockUserService
                .Setup(x => x.ValidateUser(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult<User>(null));
            
            // When
            var result = await _userController.Login(loginModel);
            
            // Then
            var badResult = Assert.IsType<UnauthorizedObjectResult>(result);
        }
    }
}