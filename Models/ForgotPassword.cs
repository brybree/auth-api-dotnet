using System.ComponentModel.DataAnnotations;

namespace AuthApi.Models
{
    /// <summary>
    /// Class <c>ForgotPassword</c> gives the required fields to request a password reset.
    /// </summary>
    public class ForgotPassword
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
    }
}