using Backend.Applications.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
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
                    var message = $"Successfully imported {success} rows.";
                    var response = new { message = message };
                    return Ok(response);
                }
                else
                {
                    var message = $"Failed to import data {success}.";
                    var response = new { message = message };
                    return StatusCode(StatusCodes.Status500InternalServerError, response);
                }
            }
            catch (Exception ex)
            {
                var message = $"Failed to import data: {ex.Message}";
                var response = new { message = message };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }


        [HttpPost]
        [Route("Stations")]
        [Authorize]
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

                int success = await _stationService.ImportStationFromCsv(filePath);
                if (success > 0)
                {
                    var response = new { message = $"Successfully imported {success} rows." };
                    return StatusCode(StatusCodes.Status200OK, response);
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
