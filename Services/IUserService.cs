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

        // TODO : may check hash instead of clear password here
        /// <summary>
        /// Return the user if he exists and matchs the authentication
        /// </summary>
        /// <param name="email">user email</param>
        /// <param name="password">user password</param>
        /// <returns></returns>
        Task<User> ValidateUser(string email, string password);
    }
}