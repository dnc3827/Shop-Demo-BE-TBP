using DEMO_Shop.Data;
using DEMO_Shop.DTOs;
using DEMO_Shop.Helpers;
using DEMO_Shop.Models;
using DEMO_Shop.Services;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace DEMO_Shop.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly CartService _cartService;
        private readonly IConfiguration _configuration;

        public AuthController(AuthService authService, CartService cartService, IConfiguration configuration)
        {
            _authService = authService;
            _cartService = cartService;
            _configuration = configuration;
        }

        // ================= REGISTER =================
        [HttpPost("register")]
        public IActionResult Register(RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                _authService.Register(dto);
                return Ok(new { message = "Đăng ký thành công" });
            }
            catch (Exception ex) when (ex.Message == "USERNAME_EXISTS")
            {
                return BadRequest(new { message = "Username đã tồn tại" });
            }
            catch (Exception ex) when (ex.Message == "EMAIL_EXISTS")
            {
                return BadRequest(new { message = "Email đã tồn tại" });
            }
        }


        // ================= LOGIN =================
        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            try
            {
                var user = _authService.Login(dto);

                if (!string.IsNullOrEmpty(dto.SessionId))
                {
                    _cartService.MergeCart(dto.SessionId, user.Id);
                }

                var token = JwtHelper.GenerateToken(user, _configuration);

                return Ok(new
                {
                    token,
                    user = new
                    {
                        user.Id,
                        user.Username,
                        user.Role
                    }
                });
            }
            catch (Exception ex) when (ex.Message == "INVALID_CREDENTIAL")
            {
                return Unauthorized(new { message = "Sai tài khoản hoặc mật khẩu" });
            }
        }

    }
}
