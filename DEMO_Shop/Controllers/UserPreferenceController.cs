using DEMO_Shop.DTOs;
using DEMO_Shop.Services;
using Microsoft.AspNetCore.Mvc;

namespace DEMO_Shop.Controllers
{
    [ApiController]
    [Route("api/preference")]
    public class UserPreferenceController : ControllerBase
    {
        private readonly UserPreferenceService _service;

        public UserPreferenceController(UserPreferenceService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult Create(UserPreferenceDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _service.Create(dto);
            return Ok(new { message = "Đã lưu thông tin" });
        }
    }
}
