namespace DEMO_Shop.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Slug { get; set; } = null!;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
