using AuthApi.Models;
using AuthAPI.Data;
using Microsoft.AspNetCore.Identity;

namespace AuthApi.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> _userManager)
        {
            this._userManager = _userManager;
        }

        /// <inheritdoc/>
        async Task<bool> IUserService.RegisterUser(Register registerModel)
        {
            var user = new ApplicationUser
            {
                UserName = registerModel.Email,
                Email = registerModel.Email,
                FirstName = registerModel.FirstName,
                LastName = registerModel.LastName,
            };

            var result = await _userManager.CreateAsync(user, registerModel.Password);
            if (result.Succeeded)
            {
                return true;
            }
            return false;
        }

        /// <inheritdoc/>
        async Task<bool> IUserService.UserExists(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user != null;
        }
    }
}