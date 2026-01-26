using LibraryAPI.DTOs;
using Microsoft.AspNetCore.Identity.Data;

namespace LibraryAPI.Service
{
    public interface IUserService
    {
        Task<UserResponse> LoginAsync(DTOs.LoginRequest request);
        Task<bool> RegisterAsync(RegisterRequest request);
    }
}
