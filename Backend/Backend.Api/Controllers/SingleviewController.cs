using Backend.Applications.Interfaces.Services;
using Backend.Domain.DTOs;
using Backend.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SingleviewController : ControllerBase
    {
        private readonly IJourneyService _journeyService;
        private readonly IStationService _stationService;

        public SingleviewController(IJourneyService journeyService, IStationService stationService)
        {
            _journeyService = journeyService;
            _stationService = stationService;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id, int? month)
        {
            var station = await _stationService.GetStationByIdAsync(id);
            if (station == null)
            {
                return NotFound();
            }

            var model = new StationDetailsDto
            {
                Name = station.Name,
                Address = station.Address,
                Location = new LocationDto
                {
                    x = station.x,
                    y = station.y
                }
            };

            if (month.HasValue)
            {
                var journeys = await _journeyService.GetJourneysByMonthAsync(month.Value);
                var departureJourneys = journeys.Where(j => j.DepartureStationId == id);
                var returnJourneys = journeys.Where(j => j.ReturnStationId == id).AsQueryable();
                model.DepartureJourneyCount = departureJourneys.Count();
                model.ReturnJourneyCount = returnJourneys.Count();
                model.AverageDistanceOfDepartureJourneys = departureJourneys.Any() ? departureJourneys.Average(j => j.CoveredDistanceInMeters) : 0;
                model.AverageDistanceOfReturnJourneys = returnJourneys.Any() ? returnJourneys.Average(j => j.CoveredDistanceInMeters) : 0;
                model.Top5ReturnStations = (GetTop5StationsAsync(returnJourneys, id))
                    .ToDictionary(station => station.ID, station => station.JourneyCount);
                model.Top5DepartureStations = (GetTop5StationsAsync(departureJourneys, id))
                    .ToDictionary(station => station.ID, station => station.JourneyCount);
            }
            else
            {
                model.DepartureJourneyCount = await _journeyService.GetDepartureJourneyCountFromStationAsync(id);
                model.ReturnJourneyCount = await _journeyService.GetReturnJourneyCountToStationAsync(id);
                model.AverageDistanceOfDepartureJourneys = await _journeyService.GetAverageDistanceOfDepartureJourneysFromStationAsync(id);
                model.AverageDistanceOfReturnJourneys = await _journeyService.GetAverageDistanceOfReturnJourneysToStationAsync(id);
                model.Top5ReturnStations = (await _journeyService.GetTop5ReturnStationsForStationAsync(id))
                .ToDictionary(pair => pair.Key.ID, pair => pair.Value);

                model.Top5DepartureStations = (await _journeyService.GetTop5DepartureStationsForStationAsync(id))
                    .ToDictionary(pair => pair.Key.ID, pair => pair.Value);
                

            }

            return Ok(model);
        }
        private List<Top5StationViewModel> GetTop5StationsAsync(IEnumerable<Journey> journeys, int stationId)
        {
            var stations = journeys
                .GroupBy(j => j.DepartureStationId == stationId ? j.ReturnStation : j.DepartureStation)
                .OrderByDescending(g => g.Count())
                .Take(5)
                .Select(g => new Top5StationViewModel
                {
                    StationName = g.Key.Name,
                    JourneyCount = g.Count(),
                    ID = g.Key.ID
                })
                .ToList();

            return stations;
        }


    }
}
