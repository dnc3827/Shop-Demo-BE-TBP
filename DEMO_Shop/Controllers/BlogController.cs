using DEMO_Shop.DTOs;
using DEMO_Shop.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DEMO_Shop.Controllers
{
    [ApiController]
    [Route("api/blogs")]
    public class BlogController : ControllerBase
    {
        private readonly BlogService _blogService;

        public BlogController(BlogService blogService)
        {
            _blogService = blogService;
        }

        // ================= PUBLIC =================

        // GET: api/blogs
        // List blog (image + title)
        [HttpGet]
        public IActionResult GetActive()
        {
            var blogs = _blogService.GetActive();
            return Ok(blogs);
        }

        // GET: api/blogs/{id}
        // Blog detail
        [HttpGet("{id}")]
        public IActionResult GetDetail(int id)
        {
            var blog = _blogService.GetById(id);

            if (blog == null || !blog.IsActive)
                return NotFound();


            var response = new BlogDetailResponseDto
            {
                BlogId = blog.BlogId,
                Title = blog.Title,
                ImageUrl = blog.ImageUrl,
                IsActive = blog.IsActive,
                Content = blog.Detail?.Content
            };

            return Ok(response);
        }

        // ================= ADMIN =================

        [Authorize(Roles = "Admin")]
        [HttpGet("admin/{id}")]
        public IActionResult GetDetailForAdmin(int id)
        {
            var blog = _blogService.GetById(id);

            if (blog == null)
                return NotFound();

            return Ok(new BlogDetailResponseDto
            {
                BlogId = blog.BlogId,
                Title = blog.Title,
                ImageUrl = blog.ImageUrl,
                IsActive = blog.IsActive,
                Content = blog.Detail?.Content
            });
        }



        // POST: api/blogs
        [HttpPost("create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(
            [FromForm] BlogCreateUpdateDto dto,
            [FromForm(Name = "imageFile")] IFormFile imageFile)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _blogService.Create(dto, imageFile);

            if (result == null)
                return BadRequest("Vui lòng upload hình ảnh");

            return Ok(result);
        }


        // PUT: api/blogs/{id}
        [HttpPut("update")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(
            [FromForm] BlogCreateUpdateDto dto,
            [FromForm(Name = "imageFile")] IFormFile? imageFile)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _blogService.Update(dto, imageFile);

            if (result == null)
                return NotFound("Blog không tồn tại");

            return Ok(result);
        }


        // DELETE: api/blogs/{id}
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var success = _blogService.Delete(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
