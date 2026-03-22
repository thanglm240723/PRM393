using AutoMapper;
using LibraryAPI.Data.Models;
using LibraryAPI.DTOs;

namespace LibraryAPI.mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
           
            CreateMap<RegisterRequest, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.Now))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.Now));

            CreateMap<User, UserResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.AvatarUrl))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
                .ForMember(dest => dest.Token, opt => opt.Ignore()); 

           
            CreateMap<Book, BookResponse>()
                .ForMember(dest => dest.TotalChapters, opt => opt.Ignore()); 

           
            CreateMap<Book, BookDetailResponse>()
                .ForMember(dest => dest.TotalChapters, opt => opt.Ignore()); 
        }
    }
}