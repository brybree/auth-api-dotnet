using AuthApi.Controllers;
using AuthApi.Models;
using AuthApi.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using System.Threading.Tasks;

namespace AuthApi.Tests.Controllers
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<ITokenService> _mockTokenService;
        private readonly UserController _userController;


        public UserControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _mockTokenService = new Mock<ITokenService>();
            _userController = new UserController(_mockUserService.Object, _mockTokenService.Object);
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

            // When
            var result = await _userController.Register(registerModel);

            // Then
            Assert.IsType<OkObjectResult>(result);
        }
    }
}