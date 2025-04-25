using System.ComponentModel.DataAnnotations;

namespace AuthApi.Models
{
    /// <summary>
    /// Class <c>Login</c> gives the required fields to log an user.
    /// </summary>
    public class Login
    {
        [Required]
        public required string UserName { get; set; }

        [Required]
        public required string Password { get; set; } 
    }
}