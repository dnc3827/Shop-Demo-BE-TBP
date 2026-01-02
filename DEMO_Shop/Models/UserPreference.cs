namespace DEMO_Shop.Models
{
    public class UserPreference
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
