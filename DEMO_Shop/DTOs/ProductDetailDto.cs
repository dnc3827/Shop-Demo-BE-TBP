namespace DEMO_Shop.DTOs
{
    public class ProductDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }

        public CategoryDto Category { get; set; } = null!;
    }
}
