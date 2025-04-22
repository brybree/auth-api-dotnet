using System.ComponentModel.DataAnnotations;

namespace AuthApi.Models
{
    /// <summary>
    /// Class <c>Register</c> gives the required fields to register a new user.
    /// </summary>
    public class Register {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [MinLength(8)]
        public required string Password { get; set; }

        [Required]
        [Compare("Password")]
        public required string ConfirmPassword { get; set; } 
        
        // others business requirements
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}