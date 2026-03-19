using LibraryAPI.Data;
using LibraryAPI.DTOs;
using LibraryAPI.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Service;

public class AdminUserService : IAdminUserService
{
    private readonly PersonalLibraryContext _context;

    public AdminUserService(PersonalLibraryContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<AdminUserResponse>> GetUsersAsync(
        int page = 1, int pageSize = 20, string? searchTerm = null)
    {
        var query = _context.Users.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var s = searchTerm.Trim();
            const string collation = "Vietnamese_CI_AI";
            query = query.Where(u =>
                EF.Functions.Collate(u.Username, collation).Contains(s) ||
                (u.Email != null && EF.Functions.Collate(u.Email, collation).Contains(s)) ||
                (u.FullName != null && EF.Functions.Collate(u.FullName, collation).Contains(s)));
        }

        var totalCount = await query.CountAsync();

        var users = await query
            .OrderByDescending(u => u.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new AdminUserResponse
            {
                UserId = u.UserId,
                Username = u.Username,
                Email = u.Email,
                FullName = u.FullName,
                AvatarUrl = u.AvatarUrl,
                Role = u.Role ?? "user",
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt,
            })
            .ToListAsync();

        return new PagedResult<AdminUserResponse>
        {
            Items = users,
            TotalCount = totalCount,
            PageNumber = page,
            PageSize = pageSize,
        };
    }

    public async Task<AdminUserResponse?> GetUserByIdAsync(int userId)
    {
        var user = await _context.Users
            .AsNoTracking()
            .Include(u => u.Stats)
            .FirstOrDefaultAsync(u => u.UserId == userId);
        if (user == null) return null;

        var response = new AdminUserResponse
        {
            UserId = user.UserId,
            Username = user.Username,
            Email = user.Email,
            FullName = user.FullName,
            AvatarUrl = user.AvatarUrl,
            Role = user.Role ?? "user",
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            UserStatsSummary = user.Stats == null ? null : new AdminUserStatsSummary
            {
                TotalBooksRead = user.Stats.TotalBooksRead,
                TotalBooksStarted = user.Stats.TotalBooksStarted,
                TotalPagesRead = user.Stats.TotalPagesRead,
                TotalMinutesRead = user.Stats.TotalMinutesRead,
                TotalWordsRead = user.Stats.TotalWordsRead,
                CurrentStreak = user.Stats.CurrentStreak,
                LongestStreak = user.Stats.LongestStreak,
                LastReadDate = user.Stats.LastReadDate,
                FavoriteGenre = user.Stats.FavoriteGenre,
                Rank = user.Stats.Rank ?? "Mầm Đọc",
                StatsUpdatedAt = user.Stats.UpdatedAt,
            },
        };
        return response;
    }

    public async Task<AdminUserResponse?> UpdateUserAsync(int userId, AdminUpdateUserRequest request)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return null;

        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            var emailExists = await _context.Users
                .AnyAsync(u => u.Email == request.Email.Trim() && u.UserId != userId);
            if (emailExists)
                throw new InvalidOperationException("Email đã được sử dụng bởi tài khoản khác.");
        }

        if (request.FullName != null) user.FullName = request.FullName.Trim();
        if (request.Email != null) user.Email = request.Email.Trim();
        if (request.AvatarUrl != null) user.AvatarUrl = request.AvatarUrl.Trim();
        if (request.Role != null) user.Role = request.Role.Trim();
        user.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();
        return await GetUserByIdAsync(userId);
    }
}
