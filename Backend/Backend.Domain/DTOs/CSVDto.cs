using Backend.Domain.Validation;
using CsvHelper.Configuration.Attributes;

namespace Backend.Domain.DTOs
{
    public class CSVDto
    {
        
        public DateTime Departure { get; set; }
        
        public DateTime Return { get; set; }
        
        public int DepartureStationId { get; set; }
        [Ignore]
        public string DepartureStation { get; set; }
      
        public int ReturnStationId { get; set; }
        [Ignore]
        public string ReturnStation { get; set; }
        public int CoveredDistanceInMeters { get; set; }
        public int DurationInSeconds { get; set; }
        public CSVDto() { }
    }
    public static class JourneyDtoExtensions
    {
        public static string[] ToStringArray(this CSVDto dto)
        {
            return new string[] {
            dto.Departure.ToString(),
            dto.Return.ToString(),
            dto.DepartureStationId.ToString(),
            //dto.DepartureStation,
            dto.ReturnStationId.ToString(),
           // dto.ReturnStation,
            dto.CoveredDistanceInMeters.ToString(),
            dto.DurationInSeconds.ToString(),
        };
        }
    }
}
