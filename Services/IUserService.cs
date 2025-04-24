using AuthApi.Models;

namespace AuthApi.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Register an user 
        /// </summary>
        /// <param name="register">The model corresponding to the user to register</param>
        /// <returns></returns>
        Task<bool> RegisterUser(Register registerModel);

        /// <summary>
        /// Check if a user exist
        /// </summary>
        /// <remarks>Whether or not the email is confirm, if the user exist in database, return true</remarks>
        /// <param name="v"></param>
        /// <returns></returns>
        Task<bool> UserExists(string email);

        /// <summary>
        /// Return the user if he exists and matchs the authentication
        /// </summary>
        /// <param name="email">user email</param>
        /// <param name="password">user password</param>
        /// <returns>null if invalid or not confirmed</returns>
        Task<User?> ValidateUser(string email, string password);

        /// <summary>
        /// Return the user corresponding to the email
        /// </summary>
        /// <param name="email"></param>
        /// <returns>null if not found</returns>
        Task<User?> GetUserByEmail(string email);

        /// <summary>
        /// Reset password
        /// </summary>
        /// <param name="resetPassword"></param>
        /// <returns></returns>
        Task<bool> ResetPassword(ResetPassword resetPassword);
    }
}