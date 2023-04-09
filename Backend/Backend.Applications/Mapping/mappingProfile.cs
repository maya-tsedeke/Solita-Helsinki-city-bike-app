using AutoMapper;
using Backend.Domain.DTOs;
using Backend.Domain.Entities;

namespace Backend.Applications.Mapping
{
    public class mappingProfile : Profile
    {
        public mappingProfile() {
            //Station Datasets
            CreateMap<StationDto, Station>().ReverseMap();
            CreateMap<addressDto, Station>().ReverseMap();
            CreateMap<SIDRequestDto, Station>();
            CreateMap<StationDetailsDto, StationDto>().ReverseMap();
            //Journy Datasets
            CreateMap<JourneyDto, Journey>().ReverseMap();
            CreateMap<JIDRequestDto, Journey>();
            CreateMap<CSVDto, Journey>().ReverseMap();
            
            //User authoruzation Dto
            CreateMap<UserDto, User>().ReverseMap();
            CreateMap<AuthenticateRequest, User>().ReverseMap();
            CreateMap<ChangePasswordDto, User>();
            CreateMap<RegisterUserDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore());
            CreateMap<RegisterUserDto, UserDto>();


        }
 
    }
}
