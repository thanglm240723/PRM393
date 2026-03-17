using LibraryAPI.DTOs;

namespace LibraryAPI.Service.Interface
{
    public interface IBookRatingService
    {
        Task<BookRatingResponse> SaveRatingAsync(int userId, SaveRatingRequest request);
        Task<BookRatingSummary> GetBookRatingSummaryAsync(int userId, int bookId);
        Task<bool> DeleteRatingAsync(int userId, int bookId);
    }
}
