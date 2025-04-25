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
        /// <remarks>Whether or not the user name is confirm, if the user exist in database, return true</remarks>
        /// <param name="v"></param>
        /// <returns></returns>
        Task<bool> UserExists(string userName);

        /// <summary>
        /// Return the user if he exists and matchs the authentication
        /// </summary>
        /// <param name="userName">user name</param>
        /// <param name="password">user password</param>
        /// <returns>null if invalid or not confirmed</returns>
        Task<User?> ValidateUser(string userName, string password);

        /// <summary>
        /// Return the user corresponding to the username
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>null if not found</returns>
        Task<User?> GetUserByName(string userName);
    }
}