using LibraryAPI.DTOs;

namespace LibraryAPI.Service.Interface
{
    public interface IQuoteService
    {
        Task<QuoteResponse> SaveQuoteAsync(int userId, SaveQuoteRequest request);
        Task<List<QuoteResponse>> GetMyQuotesAsync(int userId, int? bookId = null);
        Task<bool> DeleteQuoteAsync(int userId, int quoteId);
    }
}
