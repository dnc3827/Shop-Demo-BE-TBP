namespace DEMO_Shop.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public string ReceiverName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public string Address { get; set; }
        public string PaymentMethod { get; set; }
        public decimal Total { get; set; }

        public string Status { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

}
