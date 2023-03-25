using System;
using System.Collections.Generic;
using System.Linq;
namespace Backend.Domain.DTOs
{
    public class StationDetailsDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public LocationDto Location { get; set; }
        public int DepartureJourneyCount { get; set; }
        public int ReturnJourneyCount { get; set; }
        public double AverageDistanceOfDepartureJourneys { get; set; }
        public double AverageDistanceOfReturnJourneys { get; set; }
        public Dictionary<int, int> Top5ReturnStations { get; set; }
        public Dictionary<int, int> Top5DepartureStations { get; set; }

        public StationDetailsDto(){}
    }
}
