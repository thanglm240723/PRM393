using LibraryAPI.DTOs;

namespace LibraryAPI.Service.Interface;

public interface IAdminUserService
{
    Task<PagedResult<AdminUserResponse>> GetUsersAsync(int page = 1, int pageSize = 20, string? searchTerm = null);
    Task<AdminUserResponse?> GetUserByIdAsync(int userId);
    Task<AdminUserResponse?> UpdateUserAsync(int userId, AdminUpdateUserRequest request);
}
