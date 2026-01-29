using AutoMapper;
using LibraryAPI.Data.Models;
using LibraryAPI.DTOs;

namespace LibraryAPI.mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Map từ RegisterRequest sang User (dùng cho đăng ký)
            // Bỏ qua Password vì chúng ta cần Hash thủ công trước khi lưu
            CreateMap<RegisterRequest, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));

            // Map từ User sang UserResponse (dùng cho trả về kết quả login)
            CreateMap<User, UserResponse>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.Email));
        }
    }
}