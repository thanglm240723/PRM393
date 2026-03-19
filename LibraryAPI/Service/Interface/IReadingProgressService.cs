using LibraryAPI.Data.Models;
using LibraryAPI.DTOs;

namespace LibraryAPI.Service.Interface
{
    public interface IReadingProgressService
    {
        Task<ReadingProgressResponse?> GetProgressAsync(int userId, int bookId);

     
        Task<ReadingProgressResponse> SaveProgressAsync(int userId, SaveProgressRequest request);

        Task<List<ReadingProgressResponse>> GetAllProgressAsync(int userId);

        Task<List<ReadingHistoryDto>> GetReadingHistoryAsync(int userId);
        Task<List<BookmarkDto>> GetBookmarksAsync(int userId);
    }
}