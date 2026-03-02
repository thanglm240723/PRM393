using LibraryAPI.DTOs;

namespace LibraryAPI.Service.Interface
{
    namespace LibraryAPI.Service.Interface
    {
        public interface IReadingProgressService
        {
            Task<ReadingProgressResponse?> GetProgressAsync(int userId, int bookId);
            Task<ReadingProgressResponse> SaveProgressAsync(int userId, SaveProgressRequest request, int totalChapters);
            Task<List<ReadingProgressResponse>> GetAllProgressAsync(int userId);
        }
    }

}
