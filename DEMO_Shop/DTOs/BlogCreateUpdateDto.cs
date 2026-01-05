using System.ComponentModel.DataAnnotations;

namespace DEMO_Shop.DTOs
{
    public class BlogCreateUpdateDto
    {
        public int? BlogId { get; set; } // null = create, có giá trị = edit

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        public string ImageUrl { get; set; }

        [Required]
        public string Content { get; set; }

        public bool IsActive { get; set; }
    }
}
