using DEMO_Shop.Data;
using DEMO_Shop.Models;
using DEMO_Shop.Services;
using Microsoft.AspNetCore.Mvc;

namespace DEMO_Shop.Controllers
{
    [ApiController]
    [Route("api/cart")]
    public class CartController : ControllerBase
    {
        private readonly CartService _service;

        public CartController(CartService service)
        {
            _service = service;
        }

        // ================= GET CART =================
        [HttpGet]
        public IActionResult Get(string sessionId)
        {
            return Ok(_service.GetItems(sessionId, null));
        }

        // ================= ADD ITEM =================
        [HttpPost("add")]
        public IActionResult Add(string sessionId, int productId)
        {
            _service.AddItem(sessionId, productId, null);
            return Ok(new { message = "Đã thêm vào giỏ hàng" });
        }

        [HttpPut("update")]
        public IActionResult Update(int cartItemId, int quantity)
        {
            _service.UpdateQuantity(cartItemId, quantity);
            return Ok();
        }

        [HttpGet("total")]
        public IActionResult Total(string sessionId)
        {
            return Ok(_service.GetTotal(sessionId, null));
        }

    }
}
