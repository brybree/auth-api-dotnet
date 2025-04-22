using AuthApi.Models;

namespace AuthApi.Services
{
    public interface IUserService
    {
        Task<bool> RegisterUser(Register register);
        Task<bool> UserExists(string v);
    }
}