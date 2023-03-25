using Backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Domain.DTOs
{
    public class JourneyDto
    {
        public int Id { get; set; }
        public DateTime Departure { get; set; }
        public DateTime Return { get; set; }
        public int DepartureStationId { get; set; }
        public int ReturnStationId { get; set; }
        public int CoveredDistanceInMeters { get; set; }
        public int DurationInSeconds { get; set; }
        public StationDto DepartureStation { get; set; }
        public StationDto ReturnStation { get; set; }
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
