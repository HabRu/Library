using AutoMapper;
using BusinessLayer.Services.LibraryParser.Model;
using Library.Models;
using Library.ViewModels;

namespace Library.Profiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, AddBookViewModel>().ReverseMap();
            CreateMap<Book, BookViewModel>().ReverseMap();
            CreateMap<Book, EditBookViewModel>().ReverseMap();
            CreateMap<Book, PageViewModel>().ReverseMap();
            CreateMap<Book, BookParserModel>().ReverseMap();
        }
    }
}
