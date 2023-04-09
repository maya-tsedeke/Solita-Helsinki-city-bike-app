using Backend.Applications.Interfaces.Services;
using Backend.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StationListController : ControllerBase
    {
        private readonly IStationService _stationService; 


        public StationListController(IStationService stationService) 
        {
            _stationService = stationService;
        }
        [HttpGet("Filter")]
        public async Task<IEnumerable<StationDto>> ListStation(int limit = 100, int offset = 1, string orderBy = null, string search = null)
        {
            var journeys = await _stationService.ListStations(limit, offset, orderBy, search);
            return journeys;
        }
        [HttpGet("{stationId}")]
        public async Task<ActionResult<StationDto>> GetStation(int stationId)
        {
            var station = await _stationService.GetStation(stationId);

            if (station == null)
            {
                return NotFound();
            }

            return Ok(station);
        }
        [HttpGet]
        public async Task<ActionResult<List<addressDto>>> GetStations(string ids)
        {
            try
            {if(ids == null)
               throw new ArgumentNullException(nameof(ids));
                var idList = ids.Split(',');
                var stations = await _stationService.GetStationsByIdsAsync(idList);

                if (!stations.Any())
                {
                    return NotFound();
                }

                return Ok(stations);
            }
            catch (Exception ex)
            {
                // Log the error and return a 400 Bad Request status code
                return StatusCode(StatusCodes.Status400BadRequest, $"Invalid IDs,Failed to ´fetch data: {ex.Message}");
            }
        }
    }
}
