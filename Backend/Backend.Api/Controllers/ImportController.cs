using Backend.Applications.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportController : ControllerBase
    {
        private readonly IStationService _stationService;
        private readonly IJourneyService _journeyService;
        private readonly ILogger<ImportController> _logger;

        public ImportController(IStationService stationService, IJourneyService journeyService, ILogger<ImportController> logger)
        {
            _stationService = stationService;
            _journeyService = journeyService;
            _logger = logger;
        }
        [HttpPost] 
        [Route("Journeys")]
        public async Task<IActionResult> ImportJourneysFromCsv(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File not selected.");
            }

            if (!Path.GetExtension(file.FileName).Equals(".csv", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("Invalid file format.");
            }

            var filePath = Path.GetTempFileName();
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            try
            {

                int success = await _journeyService.ImportJourneysFromCsv(filePath);
                if (success > 0)
                {
                    return StatusCode(StatusCodes.Status200OK, $"Successfully imported {success} rows.");
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, $"Failed to import data {success} .");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Failed to import data: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("Stations")]
        public async Task<IActionResult> ImportStationsFromCsv(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File not selected.");
            }

            if (!Path.GetExtension(file.FileName).Equals(".csv", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("Invalid file format.");
            }

            var filePath = Path.GetTempFileName();
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            try
            {

                int  success= await _stationService.ImportStationFromCsv(filePath);
                if (success>0)
                {
                    return StatusCode(StatusCodes.Status200OK, $"Successfully imported {success} rows.");
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Failed to import data.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Failed to import data: {ex.Message}");
            }
        }

    }
}
