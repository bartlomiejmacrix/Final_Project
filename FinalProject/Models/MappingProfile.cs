using AutoMapper;
using FinalProject.DTOs;

namespace FinalProject.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CustomerDTO, Customer>()
                .ForMember(dest => dest.DateOfBirth, opt => opt
                .MapFrom(src => DateTime.SpecifyKind(src.DateOfBirth, DateTimeKind.Utc)))
                .ReverseMap();
        }
    }
}