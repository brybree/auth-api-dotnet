using System.ComponentModel.DataAnnotations;

namespace AuthApi.Models
{
    /// <summary>
    /// Class <c>Login</c> gives the required fields to log an user.
    /// </summary>
    public class Login
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; } 
    }
}