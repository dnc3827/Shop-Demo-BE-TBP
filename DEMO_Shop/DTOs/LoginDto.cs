namespace DEMO_Shop.DTOs
{
    public class LoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }

        // sessionId FE gửi lên
        public string? SessionId { get; set; }
    }
}
