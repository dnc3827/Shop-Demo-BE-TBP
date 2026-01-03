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
        // 1. Thêm async Task và [FromForm] để nhận file từ giao diện
        public async Task<IActionResult> Create([FromForm] DTOs.CreateProductDto dto, IFormFile? imageFile)
        {
            // 2. Truyền thêm imageFile vào Service
            await _service.Create(dto, imageFile);
            return Ok(new { message = "Tạo sản phẩm thành công" });
        }

        // ================= UPDATE (ADMIN) =================
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        // 3. Tương tự cho Update
        public async Task<IActionResult> Update(int id, [FromForm] DTOs.UpdateProductDto dto, IFormFile? imageFile)
        {
            await _service.Update(id, dto, imageFile);
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
