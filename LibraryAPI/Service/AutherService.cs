using AutoMapper;
using LibraryAPI.Data;
using LibraryAPI.Data.Models;
using LibraryAPI.DTOs;
using LibraryAPI.Service.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LibraryAPI.Service
{
    public class AutherService : IAutherService
    {
        private readonly PersonalLibraryContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AutherService(
            PersonalLibraryContext context,
            IConfiguration configuration,
            IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<UserResponse?> LoginAsync(LoginRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return null;

            var token = GenerateJwtToken(user);
            var response = _mapper.Map<UserResponse>(user);
            response.Token = token;
            return response;
        }

        public async Task<bool> RegisterAsync(RegisterRequest request)
        {
            // Kiểm tra trùng username hoặc email
            if (await _context.Users.AnyAsync(u =>
                    u.Username == request.Username || u.Email == request.Email))
                return false;

            // Tạo user mới
            var newUser = _mapper.Map<User>(request);
            newUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            newUser.Role = "user";

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync(); // Lấy UserId mới

            // ── FIX: Tạo UserStats mặc định cho user mới ─────────────
            // Bắt buộc phải có trước khi dùng gamification
            var stats = new UserStats
            {
                UserId = newUser.UserId,
                TotalBooksRead = 0,
                TotalBooksStarted = 0,
                TotalPagesRead = 0,
                TotalMinutesRead = 0,
                TotalWordsRead = 0,
                CurrentStreak = 0,
                LongestStreak = 0,
                LastReadDate = null,
                FavoriteGenre = null,
                Rank = "Mầm Đọc",
                UpdatedAt = DateTime.Now,
            };
            _context.UserStats.Add(stats);
            await _context.SaveChangesAsync();

            return true;
        }



        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name,           user.Username),
                new Claim(ClaimTypes.Email,          user.Email),
                new Claim(ClaimTypes.Role,           user.Role ?? "user"),
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature),
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }
    }
}