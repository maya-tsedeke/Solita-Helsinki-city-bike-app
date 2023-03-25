using AutoMapper;
using Backend.Domain.DTOs;
using Backend.Domain.Entities;

namespace Backend.Applications.Mapping
{
    public class mappingProfile : Profile
    {
        public mappingProfile() {
            //Station Datasets
            CreateMap<StationDto, Station>()
                .ReverseMap();
            CreateMap<SIDRequestDto, Station>();
           //Journy Datasets
            CreateMap<JourneyDto, Journey>()
                .ReverseMap();
            CreateMap<JIDRequestDto, Journey>();
            CreateMap<CSVDto, Journey>()
                .ReverseMap();
            CreateMap<StationDetailsDto, StationDto>()
              .ReverseMap();
        }
    }
}
