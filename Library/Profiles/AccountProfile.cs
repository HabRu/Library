using AutoMapper;
using Library.Models;
using Library.ViewModels;
using System.Security.Cryptography;

namespace Library.Profiles
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<User, UserRegisterViewModel>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.NameUser))
                .ReverseMap();
        }
    }
}
