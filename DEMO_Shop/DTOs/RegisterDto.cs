using System.ComponentModel.DataAnnotations;

namespace DEMO_Shop.DTOs
{
    public class RegisterDto
    {
        [Required, MinLength(4)]
        public string Username { get; set; } = null!;

        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required, MinLength(6)]
        public string Password { get; set; } = null!;

        [Required, Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = null!;
    }
}
