using AuthApi.Models;
using AuthApi.Services;
using AuthAPI.Data;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace AuthApi.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            // Setup mock UserManager
            var store = new Mock<IUserStore<ApplicationUser>>();
            #pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            _mockUserManager = new Mock<UserManager<ApplicationUser>>(
                store.Object, null, null, null, null, null, null, null, null);

            
            _userService = new UserService(_mockUserManager.Object);
        }

        [Fact]
        public async Task RegisterUser_SuccessfulRegistration_ReturnsTrue()
        {
            // Given
            var register = new Register
            {
                UserName = "testuser",
                Password = "P@ssw0rd!",
                ConfirmPassword = "P@ssw0rd"
            };

            _mockUserManager
                .Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            // When
            var result = await ((IUserService)_userService).RegisterUser(register);

            // Then
            Assert.True(result);
            _mockUserManager.Verify(
                um => um.CreateAsync(
                    It.Is<ApplicationUser>(u => u.UserName == register.UserName),
                    register.Password),
                Times.Once);
        }

        [Fact]
        public async Task RegisterUser_FailedRegistration_ReturnsFalse()
        {
            // Given
            var register = new Register
            {
                UserName = "testuser",
                Password = "P@ssw0rd!",
                ConfirmPassword = "P@ssw0rd"
            };

            _mockUserManager
                .Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Error" }));

            // When
            var result = await ((IUserService)_userService).RegisterUser(register);

            // Then
            Assert.False(result);
        }

        [Fact]
        public async Task UserExists_UserFound_ReturnsTrue()
        {
            // Given
            var userName = "existinguser";
            var user = new ApplicationUser { UserName = userName };
            
            _mockUserManager
                .Setup(um => um.FindByNameAsync(userName))
                .ReturnsAsync(user);

            // When
            var result = await ((IUserService)_userService).UserExists(userName);

            // Then
            Assert.True(result);
        }

        [Fact]
        public async Task UserExists_UserNotFound_ReturnsFalse()
        {
            // Given
            string userName = "nonexistentuser";

            _mockUserManager
                .Setup(um => um.FindByNameAsync(userName))
                .ReturnsAsync((ApplicationUser?)null);

            // When
            var result = await ((IUserService)_userService).UserExists(userName);

            // Then
            Assert.False(result);
        }

        [Fact]
        public async Task ValidateUser_ValidCredentials_ReturnsUser()
        {
            // Given
            var userName = "validuser";
            var password = "P@ssw0rd!";
            var userId = "user123";
            var applicationUser = new ApplicationUser { Id = userId, UserName = userName };
            
            _mockUserManager
                .Setup(um => um.FindByNameAsync(userName))
                .ReturnsAsync(applicationUser);
            
            _mockUserManager
                .Setup(um => um.CheckPasswordAsync(applicationUser, password))
                .ReturnsAsync(true);

            // When
            var result = await _userService.ValidateUser(userName, password);

            // Then
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
            Assert.Equal(userName, result.UserName);
        }

        [Fact]
        public async Task ValidateUser_UserNotFound_ReturnsNull()
        {
            // Given
            var userName = "invaliduser";
            var password = "P@ssw0rd!";
            
            _mockUserManager
                .Setup(um => um.FindByNameAsync(userName))
                .ReturnsAsync((ApplicationUser?)null);

            // When
            var result = await _userService.ValidateUser(userName, password);

            // Then
            Assert.Null(result);
        }

        [Fact]
        public async Task ValidateUser_InvalidPassword_ReturnsNull()
        {
            // Given
            var userName = "validuser";
            var password = "wrong_password";
            var applicationUser = new ApplicationUser { Id = "user123", UserName = userName };
            
            _mockUserManager
                .Setup(um => um.FindByNameAsync(userName))
                .ReturnsAsync(applicationUser);
            
            _mockUserManager
                .Setup(um => um.CheckPasswordAsync(applicationUser, password))
                .ReturnsAsync(false);

            // When
            var result = await _userService.ValidateUser(userName, password);

            // Then
            Assert.Null(result);
        }

        [Fact]
        public async Task GetUserByName_UserFound_ReturnsUser()
        {
            // Given
            var userName = "existinguser";
            var userId = "user123";
            var refreshToken = "refresh_token_value";
            var expiryTime = DateTime.UtcNow.AddDays(7);
            
            var applicationUser = new ApplicationUser 
            { 
                Id = userId, 
                UserName = userName,
                RefreshToken = refreshToken,
                RefreshTokenExpiryTime = expiryTime
            };
            
            _mockUserManager
                .Setup(um => um.FindByNameAsync(userName))
                .ReturnsAsync(applicationUser);

            // When
            var result = await _userService.GetUserByName(userName);

            // Then
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
            Assert.Equal(userName, result.UserName);
            Assert.Equal(refreshToken, result.RefreshToken);
            Assert.Equal(expiryTime, result.RefreshTokenExpirationTime);
        }

        [Fact]
        public async Task GetUserByName_UserNotFound_ReturnsNull()
        {
            // Given
            var userName = "nonexistentuser";
            
            _mockUserManager
                .Setup(um => um.FindByNameAsync(userName))
                .ReturnsAsync((ApplicationUser?)null);

            // When
            var result = await _userService.GetUserByName(userName);

            // Then
            Assert.Null(result);
        }
    }
}
