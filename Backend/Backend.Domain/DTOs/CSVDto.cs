using AutoMapper.Configuration.Annotations;

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
}
