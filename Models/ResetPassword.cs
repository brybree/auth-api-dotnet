using System.ComponentModel.DataAnnotations;

namespace AuthApi.Models
{
    /// <summary>
    /// Class <c>ResetPassword</c> gives the required fields to reset a password.
    /// </summary>
    public class ResetPassword
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        
        [Required]
        public required string Token { get; set; }

        [Required]
        [MinLength(8)]
        public required string Password { get; set; }

        [Required]
        [Compare("Password")]
        public required string ConfirmPassword { get; set; }
    }
}