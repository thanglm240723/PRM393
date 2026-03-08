using LibraryAPI.DTOs;

namespace LibraryAPI.Service.Interface
{
    public interface IAdminBookService
    {
        Task<PagedResult<BookResponse>> GetBooksAsync(int page = 1, int pageSize = 20, string? searchTerm = null);
        Task<CreateBookResponse> CreateBookAsync(CreateBookRequest request);
        Task<BookResponse?> GetBookByIdAsync(int bookId);
        Task<BookResponse?> UpdateBookAsync(int bookId, UpdateBookRequest request);
        Task<bool> DeleteBookAsync(int bookId);
        Task<bool> BookExistsAsync(string title, string author);
    }
}
