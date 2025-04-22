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
        Task<bool> UserExists(string v);
    }
}