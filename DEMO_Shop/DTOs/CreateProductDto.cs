namespace DEMO_Shop.DTOs
{
    public class CreateProductDto
    {
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }
    }
}
