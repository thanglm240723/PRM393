using LibraryAPI.DTOs;

namespace LibraryAPI.Service.Interface
{
    public interface IAutherService
    {
      
        Task<UserResponse?> LoginAsync(LoginRequest request);
        Task<bool> RegisterAsync(RegisterRequest request);
    }
}