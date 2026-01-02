using DEMO_Shop.Data;
using DEMO_Shop.Models;
using DEMO_Shop.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DEMO_Shop.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _service;

        public ProductController(ProductService service)
        {
            _service = service;
        }

        // ================= GET LIST + PAGING =================
        [HttpGet]
        public IActionResult Get(int page = 1, int size = 10)
        {
            var data = _service.GetPaged(page, size);

            Console.WriteLine("PRODUCT COUNT: " + data.Count);

            return Ok(data);
        }


        // ================= GET DETAIL =================
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var product = _service.GetById(id);
            if (product == null) return NotFound();

            return Ok(product);
        }

        // ================= SEARCH =================
        [HttpGet("search")]
        public IActionResult Search(string keyword)
        {
            return Ok(_service.Search(keyword));
        }

        // ================= CREATE (ADMIN) =================
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(DTOs.CreateProductDto dto)
        {
            _service.Create(dto);
            return Ok(new { message = "Tạo sản phẩm thành công" });
        }

        // ================= UPDATE (ADMIN) =================
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult Update(int id, DTOs.UpdateProductDto dto)
        {
            _service.Update(id, dto);
            return Ok(new { message = "Cập nhật thành công" });
        }

        // ================= DELETE (ADMIN) =================
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return Ok(new { message = "Xóa thành công" });
        }

        [Authorize]
        [HttpGet("auth-debug")]
        public IActionResult AuthDebug()
        {
            return Ok("AUTH OK");
        }
    }
}
