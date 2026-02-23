using LibraryAPI.DTOs;

namespace LibraryAPI.Service.Interface
{
    public interface IBookService
    {
        Task<PagedResult<BookResponse>> GetBooksAsync(int pageNumber, int pageSize);
        Task<PagedResult<BookResponse>> SearchBooksAsync(BookSearchRequest request);
        Task<BookDetailResponse?> GetBookByIdAsync(int bookId);
        Task<List<ChapterListItem>> GetChapterListAsync(int bookId);
        Task<ChapterResponse?> GetChapterAsync(int bookId, int chapterNumber);
    }
}
     