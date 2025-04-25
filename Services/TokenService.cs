using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AuthApi.Models;
using Microsoft.IdentityModel.Tokens;

namespace AuthApi.Services
{
    public class TokenService : ITokenService
    {
        /// <inheritdoc/>
        public string GenerateAccessToken(User user)
        {
            // todo: random key
            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("randomkey")
            );
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);
        
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
            };

            var token = new JwtSecurityToken(
                issuer: "issuer",
                audience: "audience",
                claims: claims,
                expires: DateTime.Now.AddMinutes(300),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);   
        }

        /// <inheritdoc/>
        public string GenerateRefreshToken()
        {
            // todo: secure random token
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}