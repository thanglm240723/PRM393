using LibraryAPI.DTOs;
using Microsoft.AspNetCore.Identity.Data;

namespace LibraryAPI.Service
{
    public interface IAutherService
    {
        Task<UserResponse> LoginAsync(DTOs.LoginRequest request);
        Task<bool> RegisterAsync(DTOs.RegisterRequest request);
    }
}
