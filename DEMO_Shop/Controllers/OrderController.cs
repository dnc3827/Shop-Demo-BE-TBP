using DEMO_Shop.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DEMO_Shop.Controllers
{
    [ApiController]
    [Route("api/order")]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _service;

        public OrderController(OrderService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpGet("my")]
        public IActionResult MyOrders()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return Ok(_service.GetUserOrders(userId));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult All()
        {
            return Ok(_service.GetAllOrders());
        }

        [Authorize]
        [HttpPost("checkout")]
        public IActionResult Checkout([FromBody] CheckoutRequest req)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var order = _service.CreateOrderFromItems(
                userId,
                req.ReceiverName,
                req.Email,
                req.Phone,
                req.Address,
                req.PaymentMethod,
                req.Items
            );

            return Ok(order);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/status")]
        public IActionResult UpdateStatus(int id, string status)
        {
            _service.UpdateStatus(id, status);
            return Ok(new { message = "Cập nhật trạng thái thành công" });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return Ok(new { message = "Xóa đơn hàng thành công" });
        }
    }
}
