using LibraryAPI.DTOs;
using Microsoft.AspNetCore.Identity.Data;

namespace LibraryAPI.Service.Interface
{
    public interface IAutherService
    {
        Task<UserResponse> LoginAsync(DTOs.LoginRequest request);
        Task<bool> RegisterAsync(DTOs.RegisterRequest request);
    }
}
