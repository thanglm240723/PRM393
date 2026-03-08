using LibraryAPI.DTOs;

namespace LibraryAPI.Service.Interface
{
    public interface IUserLibraryService
    {
        Task<bool> ToggleFavoriteAsync(int userId, int bookId);
        Task<List<BookResponse>> GetSavedBooksAsync(int userId);
        Task<bool> CheckIsSavedAsync(int userId, int bookId);
    }
}