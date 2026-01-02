using DEMO_Shop.Data;
using Microsoft.EntityFrameworkCore;

namespace DEMO_Shop.Services
{
    public class DashboardService
    {
        private readonly AppDbContext _db;

        public DashboardService(AppDbContext db)
        {
            _db = db;
        }

        public DashboardDto GetDashboard()
        {
            // ===== Chỉ lấy đơn đã thanh toán =====
            var paidOrders = _db.Orders
                .Where(o => o.Status == "Hoàn tất");

            // ===== Tổng doanh thu =====
            var totalRevenue = paidOrders.Sum(o => (decimal?)o.Total) ?? 0;

            // ===== Tổng số đơn =====
            var totalOrders = paidOrders.Count();

            // ===== Tổng số sản phẩm bán ra =====
            var totalItemsSold = _db.OrderItems
                .Where(oi => paidOrders.Any(o => o.Id == oi.OrderId))
                .Sum(oi => (int?)oi.Quantity) ?? 0;

            // ===== Doanh thu theo tháng =====
            var monthlyRevenue = paidOrders
                .GroupBy(o => o.CreatedAt.Month)
                .Select(g => new MonthlyRevenueDto
                {
                    Month = g.Key,
                    Revenue = g.Sum(x => x.Total)
                })
                .OrderBy(x => x.Month)
                .ToList();

            return new DashboardDto
            {
                TotalRevenue = totalRevenue,
                TotalOrders = totalOrders,
                TotalItemsSold = totalItemsSold,
                MonthlyRevenue = monthlyRevenue
            };
        }
    }

    // ================= DTO =================

    public class DashboardDto
    {
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public int TotalItemsSold { get; set; }
        public List<MonthlyRevenueDto> MonthlyRevenue { get; set; }
    }

    public class MonthlyRevenueDto
    {
        public int Month { get; set; }
        public decimal Revenue { get; set; }
    }
}
