using DEMO_Shop.Models;
using DEMO_Shop.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DEMO_Shop.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _service;

        public CategoryController(CategoryService service)
        {
            _service = service;
        }

        // ================= GET ALL =================
        [HttpGet]
        public IActionResult GetAll()
        {
            var data = _service.GetAll();
            return Ok(data);
        }

        // ================= GET BY ID =================
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var category = _service.GetById(id);
            if (category == null) return NotFound();

            return Ok(category);
        }

        // ================= CREATE (ADMIN) =================
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(CreateCategoryDto dto)
        {
            _service.Create(dto);
            return Ok(new { message = "Tạo danh mục thành công" });
        }

        // ================= UPDATE (ADMIN) =================
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateCategoryDto dto)
        {
            _service.Update(id, dto);
            return Ok(new { message = "Cập nhật danh mục thành công" });
        }

        // ================= DELETE (ADMIN) =================
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return Ok(new { message = "Xóa danh mục thành công" });
        }
    }
}
