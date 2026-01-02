using DEMO_Shop.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DEMO_Shop.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/dashboard")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : ControllerBase
    {
        private readonly DashboardService _service;

        public DashboardController(DashboardService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetDashboard()
        {
            var data = _service.GetDashboard();

            return Ok(new
            {
                totalRevenue = data.TotalRevenue,
                totalOrders = data.TotalOrders,
                totalItemsSold = data.TotalItemsSold,
                monthlyRevenue = data.MonthlyRevenue.Select(m => new
                {
                    month = m.Month,
                    revenue = m.Revenue
                })
            });
        }
    }
}
