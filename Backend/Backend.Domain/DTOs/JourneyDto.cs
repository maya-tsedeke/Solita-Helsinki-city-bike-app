namespace Backend.Domain.DTOs
{
    public class JourneyDto
    {
        public int Id { get; set; }
        public DateTime? Departure { get; set; }
        public DateTime? Return { get; set; }
        public int? DepartureStationId { get; set; }
        public int? ReturnStationId { get; set; }
        public double? CoveredDistanceInMeters { get; set; }
        public double? DurationInSeconds { get; set; }
        public int UserId { get; set; }
        public StationDto DepartureStation { get; set; }
        public StationDto ReturnStation { get; set; }
        public UserDto users { get; set; }
        public JourneyDto() { }

    }
    public class JIDRequestDto
    {
        public int Id { get; set; }
        public JIDRequestDto() { }
    }
    public class Top5StationViewModel
    {
        public int ID { get; set; }
        public string StationName { get; set; }
        public int JourneyCount { get; set; }
    }
}
