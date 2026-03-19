using LibraryAPI.DTOs;
using LibraryAPI.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers;

[Route("api/admin/users")]
[ApiController]
[Authorize(Roles = "admin")]
public class AdminUserController : ControllerBase
{
    private readonly IAdminUserService _adminUserService;

    public AdminUserController(IAdminUserService adminUserService)
    {
        _adminUserService = adminUserService;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? searchTerm = null)
    {
        var result = await _adminUserService.GetUsersAsync(page, pageSize, searchTerm);
        return Ok(result);
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUser(int userId)
    {
        var user = await _adminUserService.GetUserByIdAsync(userId);
        if (user == null)
            return NotFound(new { message = $"Không tìm thấy user ID = {userId}" });
        return Ok(user);
    }

    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateUser(int userId, [FromBody] AdminUpdateUserRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var result = await _adminUserService.UpdateUserAsync(userId, request);
            if (result == null)
                return NotFound(new { message = $"Không tìm thấy user ID = {userId}" });
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }
}
