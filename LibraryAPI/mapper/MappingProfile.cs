using AutoMapper;
using LibraryAPI.Data.Models;
using LibraryAPI.DTOs;

namespace LibraryAPI.mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Map từ RegisterRequest sang User
            CreateMap<RegisterRequest, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));

            // Map từ User sang UserResponse
            CreateMap<User, UserResponse>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.Email));

            // Map Book -> BookResponse (danh sách)
            CreateMap<Book, BookResponse>();

            // Map Book -> BookDetailResponse (chi tiết)
            // TotalChapters được gán thủ công trong Service vì cần query DB
            CreateMap<Book, BookDetailResponse>()
                .ForMember(dest => dest.TotalChapters, opt => opt.Ignore());
        }
    }
}