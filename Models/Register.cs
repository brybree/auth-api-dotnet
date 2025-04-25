using System.ComponentModel.DataAnnotations;

namespace AuthApi.Models
{
    /// <summary>
    /// Class <c>Register</c> gives the required fields to register a new user.
    /// </summary>
    public class Register {
        [Required]
        public required string UserName { get; set; }

        [Required]
        [MinLength(8)]
        public required string Password { get; set; }

        [Required]
        [Compare("Password")]
        public required string ConfirmPassword { get; set; } 
    }
}