namespace LibraryAPI.Service
{
    using LibraryAPI.Data;
    using LibraryAPI.Data.Models;
    using LibraryAPI.DTOs;
    using LibraryAPI.Service.Interface;
    using Microsoft.EntityFrameworkCore;

    public class QuoteService : IQuoteService
    {
        private readonly PersonalLibraryContext _context;

        public QuoteService(PersonalLibraryContext context)
        {
            _context = context;
        }

        public async Task<QuoteResponse> SaveQuoteAsync(int userId, SaveQuoteRequest request)
        {
            var quote = new Quote
            {
                UserId = userId,
                BookId = request.BookId,
                ContentId = request.ContentId,
                QuoteText = request.QuoteText.Trim(),
                PersonalNote = request.PersonalNote?.Trim(),
                StartPosition = request.StartPosition,
                EndPosition = request.EndPosition,
                IsPublic = request.IsPublic,
                CreatedAt = DateTime.Now,
            };

            _context.Quotes.Add(quote);
            await _context.SaveChangesAsync();

            return await BuildResponseAsync(quote);
        }

        public async Task<List<QuoteResponse>> GetMyQuotesAsync(int userId, int? bookId = null)
        {
            var query = _context.Quotes
                .Where(q => q.UserId == userId);

            if (bookId.HasValue)
                query = query.Where(q => q.BookId == bookId.Value);

            var quotes = await query
                .OrderByDescending(q => q.CreatedAt)
                .ToListAsync();

            var responses = new List<QuoteResponse>();
            foreach (var q in quotes)
                responses.Add(await BuildResponseAsync(q));

            return responses;
        }

        public async Task<bool> DeleteQuoteAsync(int userId, int quoteId)
        {
            var quote = await _context.Quotes
                .FirstOrDefaultAsync(q => q.QuoteId == quoteId && q.UserId == userId);

            if (quote == null) return false;

            _context.Quotes.Remove(quote);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<QuoteResponse> BuildResponseAsync(Quote q)
        {
            var book = await _context.Books.FindAsync(q.BookId);
            BookContent? chapter = null;
            if (q.ContentId.HasValue)
                chapter = await _context.BookContents.FindAsync(q.ContentId.Value);

            return new QuoteResponse
            {
                QuoteId = q.QuoteId,
                BookId = q.BookId,
                BookTitle = book?.Title,
                BookCover = book?.CoverImageUrl,
                ContentId = q.ContentId,
                ChapterTitle = chapter?.ChapterTitle,
                QuoteText = q.QuoteText,
                PersonalNote = q.PersonalNote,
                IsPublic = q.IsPublic,
                CreatedAt = q.CreatedAt,
            };
        }
    }
}
