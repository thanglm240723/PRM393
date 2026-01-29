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
    // Lưu ý: Đảm bảo Interface của bạn tên là IAuthService (như đã hướng dẫn đổi tên ở bước trước)
    // Nếu chưa đổi tên file Interface, hãy sửa IAuthService thành IAutherService
    public class AutherService : IAutherService
    {
        private readonly PersonalLibraryContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper; // Inject AutoMapper

        public AutherService(PersonalLibraryContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<UserResponse> LoginAsync(LoginRequest request)
        {
            // 1. Tìm user trong DB theo Username
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

            // 2. Kiểm tra nếu user không tồn tại hoặc password sai (dùng BCrypt để verify)
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return null; // Login thất bại
            }

            // 3. Tạo Token
            var token = GenerateJwtToken(user);

            // 4. Dùng AutoMapper để map tự động từ User entity sang UserResponse DTO
            // (Không cần viết thủ công user.UserId = response.id, ...)
            var response = _mapper.Map<UserResponse>(user);

            // 5. Gán token vào response (vì token được sinh ra, không nằm trong DB)
            response.Token = token;

            return response;
        }

        public async Task<bool> RegisterAsync(RegisterRequest request)
        {
            // 1. Kiểm tra xem username hoặc email đã tồn tại chưa
            if (await _context.Users.AnyAsync(u => u.Username == request.Username || u.Email == request.Email))
            {
                return false;
            }

            // 2. Dùng AutoMapper để chuyển đổi từ RegisterRequest sang User entity
            // (Nó sẽ tự map Username, Email, FullName...)
            var newUser = _mapper.Map<User>(request);

            // 3. Hash password và gán thủ công (Mapper đã được cấu hình ignore field này để bảo mật)
            newUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // Note: Các trường CreatedAt, UpdatedAt sẽ được xử lý trong MappingProfile hoặc để SQL tự sinh

            // 4. Lưu vào cơ sở dữ liệu
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return true;
        }

        // Hàm hỗ trợ tạo JWT Token (Private)
        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7), // Token hết hạn sau 7 ngày
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}