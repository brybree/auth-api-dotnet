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
                UserName = registerModel.UserName,
            };

            var result = await _userManager.CreateAsync(user, registerModel.Password);
            if (result.Succeeded)
            {
                return true;
            }
            return false;
        }

        /// <inheritdoc/>
        async Task<bool> IUserService.UserExists(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            return user != null;
        }

        /// <inheritdoc/>
        public async Task<User?> ValidateUser(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
                return null;
            
            return new User
            {
                Id = user.Id,
                UserName = user.UserName!,
            };
        }

        public async Task<User?> GetUserByName(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null) 
                return null;
            
            return new User
            {
                Id = user.Id,
                UserName = user.UserName!,
                RefreshToken = user.RefreshToken,
                RefreshTokenExpirationTime = user.RefreshTokenExpiryTime
            };
        }
    }
}