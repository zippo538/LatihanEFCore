using AutoMapper;
using LatihanEFCore.DTOs;
using LatihanEFCore.DTOs;

namespace LatihanEFCore.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, UserDTO>();
            CreateMap<RegisterDTO, ApplicationUser>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
        }
    }
}