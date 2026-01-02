using System.ComponentModel.DataAnnotations;

namespace DEMO_Shop.DTOs
{
    public class UserPreferenceDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Phone { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string? Description { get; set; }
    }
}
