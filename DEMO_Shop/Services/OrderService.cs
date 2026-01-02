using DEMO_Shop.Data;
using DEMO_Shop.Models;
using Microsoft.EntityFrameworkCore;

namespace DEMO_Shop.Services
{
    public class OrderService
    {
        private readonly AppDbContext _db;

        public OrderService(AppDbContext db)
        {
            _db = db;
        }

        public List<Order> GetUserOrders(int userId)
        {
            return _db.Orders
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.Id)
                .ToList();
        }

        public List<Order> GetAllOrders()
        {
            return _db.Orders
                .OrderByDescending(x => x.Id)
                .ToList();
        }

        public Order CreateOrderFromItems(
            int userId,
            string receiverName,
            string email,
            string phone,
            string address,
            string paymentMethod,
            List<CartItemDto> items
        )
        {
            if (items == null || !items.Any())
                throw new Exception("Giỏ hàng trống");

            if (!_db.Users.Any(u => u.Id == userId))
                throw new Exception("User không tồn tại");

            var productIds = items.Select(x => x.ProductId).ToList();

            var products = _db.Products
                .Where(p => productIds.Contains(p.Id))
                .ToDictionary(p => p.Id);

            if (products.Count != items.Count)
                throw new Exception("Có sản phẩm không tồn tại");

            using var transaction = _db.Database.BeginTransaction();

            try
            {
                var order = new Order
                {
                    UserId = userId,
                    ReceiverName = receiverName,
                    Email = email,
                    Phone = phone,
                    Address = address,
                    PaymentMethod = paymentMethod,
                    Status = "Pending",
                    CreatedAt = DateTime.Now,
                    Total = items.Sum(i => i.Quantity * products[i.ProductId].Price)
                };

                _db.Orders.Add(order);
                _db.SaveChanges();

                var orderItems = items.Select(i => new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = products[i.ProductId].Price
                }).ToList();

                _db.OrderItems.AddRange(orderItems);
                _db.SaveChanges();

                transaction.Commit();
                return order;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public void UpdateStatus(int orderId, string status)
        {
            var order = _db.Orders.Find(orderId);
            if (order == null) throw new Exception("Đơn hàng không tồn tại");

            order.Status = status;
            _db.SaveChanges();
        }

        public void Delete(int orderId)
        {
            var order = _db.Orders.Find(orderId);
            if (order == null) throw new Exception("Đơn hàng không tồn tại");

            if (order.Status != "Pending")
                throw new Exception("Chỉ được xóa đơn Pending");

            _db.Orders.Remove(order);
            _db.SaveChanges();
        }
    }

    // ================= DTO =================
    public class CheckoutRequest
    {
        public string ReceiverName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public string Address { get; set; }
        public string PaymentMethod { get; set; }
        public List<CartItemDto> Items { get; set; }
    }

    public class CartItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
