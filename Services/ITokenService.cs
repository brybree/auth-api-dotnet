using AuthApi.Models;

namespace AuthApi.Services
{
    public interface ITokenService
    {
        /// <summary>
        /// Generate access token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        string GenerateAccessToken(User user);
        
        /// <summary>
        /// Generate a base64 refresh token
        /// </summary>
        /// <returns></returns>
        string GenerateRefreshToken();

        /// <summary>
        /// Generate a base64 password reset token
        /// </summary>
        /// <returns></returns>
        string GeneratePasswordResetToken(User user);
    }
}