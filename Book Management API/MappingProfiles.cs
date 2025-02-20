using AutoMapper;
using Book_Management_API.DTOs;
using Book_Management_API.Models;

namespace Book_Management_API
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Book, BookDTO>().ReverseMap();
        }
    }
}
