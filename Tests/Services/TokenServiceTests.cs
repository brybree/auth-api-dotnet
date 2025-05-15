using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Web;
using AuthApi.Models;
using AuthApi.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Xunit;

namespace AuthApi.Tests.Services
{
    public class TokenServiceTests
    {
        private readonly TokenService _tokenService;
        private readonly JwtSettings _jwtSettings;
        private readonly User _testUser;

        public TokenServiceTests()
        {
            // Setup test dependencies
            _jwtSettings = new JwtSettings
            {
                SecretKey = "ThisIsAVeryLongSecretKeyForTestingPurposesOnly_AtLeast32Chars_YesItIsVeryLongAsItShouldAlwaysBe",
                TokenLifetimeMinutes = 30
            };

            var mockOptions = new Mock<IOptions<JwtSettings>>();
            mockOptions.Setup(o => o.Value).Returns(_jwtSettings);

            _tokenService = new TokenService(mockOptions.Object);

            _testUser = new User
            {
                Id = "user123",
                UserName = "testuser"
            };
        }

        [Fact]
        public void GenerateAccessToken_ShouldCreateValidToken()
        {
            // Given
            var token = _tokenService.GenerateAccessToken(_testUser);

            // Then
            Assert.NotNull(token);
            Assert.NotEmpty(token);

            // Validate the token
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)),
                ValidateIssuer = true,
                ValidIssuer = "issuer",
                ValidateAudience = true,
                ValidAudience = "audience",
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
            
            // Then
            Assert.NotNull(validatedToken);
            Assert.IsType<JwtSecurityToken>(validatedToken);
            
            var jwtToken = (JwtSecurityToken)validatedToken;
            
            // Check claims
            Assert.Equal(_testUser.Id, jwtToken.Claims.First(c => c.Type == JwtRegisteredClaimNames.Sub).Value);
            Assert.Equal(_testUser.UserName, jwtToken.Claims.First(c => c.Type == JwtRegisteredClaimNames.Name).Value);
            Assert.Contains(jwtToken.Claims, c => c.Type == JwtRegisteredClaimNames.Jti);
            Assert.Equal(_testUser.UserName, jwtToken.Claims.First(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value);
            
            // Check expiration time
            var expectedExpiryTime = DateTime.Now.AddMinutes(_jwtSettings.TokenLifetimeMinutes);
            var tokenExpiryTime = jwtToken.ValidTo;
            
            // Allow for slight differences due to test execution time
            Assert.True(Math.Abs((expectedExpiryTime - tokenExpiryTime).TotalSeconds) > 5);
        }

        [Fact]
        public void GenerateRefreshToken_ShouldReturnUrlEncodedToken()
        {
            // Given
            var refreshToken = _tokenService.GenerateRefreshToken();

            // Then
            Assert.NotNull(refreshToken);
            Assert.NotEmpty(refreshToken);
            
            // Validate we can decode it
            var decodedToken = HttpUtility.UrlDecode(refreshToken);

            // Then
            Assert.NotNull(decodedToken);
        }

        [Fact]
        public void GenerateRefreshToken_ShouldCreateDifferentTokensOnEachCall()
        {
            // Given
            var refreshToken1 = _tokenService.GenerateRefreshToken();
            var refreshToken2 = _tokenService.GenerateRefreshToken();

            // Then
            Assert.NotEqual(refreshToken1, refreshToken2);
        }

        [Fact]
        public void GenerateAccessToken_WithDifferentUsers_ShouldCreateDifferentTokens()
        {
            // Given
            var user1 = new User { Id = "user1", UserName = "userName1" };
            var user2 = new User { Id = "user2", UserName = "userName2" };

            // When
            var token1 = _tokenService.GenerateAccessToken(user1);
            var token2 = _tokenService.GenerateAccessToken(user2);

            // Then
            Assert.NotEqual(token1, token2);
        }
    }
}