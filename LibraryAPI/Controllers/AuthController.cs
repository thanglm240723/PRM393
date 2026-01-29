using LibraryAPI.DTOs;
using LibraryAPI.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAutherService _authService;

        public AuthController(IAutherService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authService.LoginAsync(request);
            if (result == null)
            {
                return Unauthorized("Sai tên đăng nhập hoặc mật khẩu.");
            }
            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _authService.RegisterAsync(request);
            if (!result)
            {
                return BadRequest("Tên đăng nhập hoặc Email đã tồn tại.");
            }
            return Ok("Đăng ký thành công.");
        }
    }
}