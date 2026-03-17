using LibraryAPI.DTOs;

namespace LibraryAPI.Service.Interface
{
    public interface IGamificationService
    {
       
        Task<GamificationResult> ProcessReadingProgressAsync(
            int userId,
            int bookId,
            decimal progressPercentage,
            string? bookGenre,
            int? bookPageCount);
    
        Task<UserStatsResponse> GetUserStatsAsync(int userId);     
        Task<List<BadgeDto>> GetUserBadgesAsync(int userId);

        Task<LeaderboardResponse> GetLeaderboardAsync(string type, int? currentUserId, int top = 20);
    }
}