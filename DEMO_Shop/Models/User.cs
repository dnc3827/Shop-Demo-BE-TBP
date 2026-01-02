namespace DEMO_Shop.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }

        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; }
        public string Role { get; set; } = "Customer";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
