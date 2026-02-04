using LibraryAPI.DTOs;

namespace LibraryAPI.Service.Interface
{
    public interface IBookService
    {
        Task<PagedResult<BookResponse>> GetBooksAsync(int pageNumber, int pageSize);
        Task<PagedResult<BookResponse>> SearchBooksAsync(BookSearchRequest request);

    }
}
