namespace DEMO_Shop.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public string? SessionId { get; set; }
        public int? UserId { get; set; }
    }
}
