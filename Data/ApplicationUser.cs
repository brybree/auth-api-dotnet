using Microsoft.AspNetCore.Identity;

namespace AuthAPI.Data
{
    /// <summary>
    /// Application user implementing <seealso cref="IdentityUser"/>
    /// </summary>
    /// <remarks>User as it will appear in the database.</remarks>
    public class ApplicationUser : IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}