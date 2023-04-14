using Backend.Applications.Interfaces.Services;
using Backend.Domain.DTOs;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StationController : ControllerBase
    {
        private readonly IStationService _stationService;
        private readonly IValidator<StationDto> _validator;
        public StationController(IStationService stationService, IValidator<StationDto> validator = null)
        {
            _stationService = stationService;
            _validator = validator;
        }
        [HttpPost("Create")]
        [Authorize]
        public async Task<IActionResult> CreateStation([FromBody] StationDto stationDto)
        {
            try
            {
                // Validate the stationDto object if the _validator is not null
                if (_validator != null)
                {
                    var validationResult = await _validator.ValidateAsync(stationDto);
                    if (!validationResult.IsValid)
                    {
                        var validationErrors = validationResult.Errors.Select(error => $"{error.PropertyName}: {error.ErrorMessage}");
                        var errorMessage = string.Join("\n", validationErrors);
                        return BadRequest(new { message = errorMessage });
                    }
                }

                var station = await _stationService.CreateStationAsync(stationDto);

                return CreatedAtAction(nameof(GetStation), new { id = station.ID }, station);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetStation(int id)
        {
            try
            {
                var station = await _stationService.GetStationByIdAsync(id);

                if (station == null)
                {
                    return NotFound();
                }

                return Ok(station);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateStation(int id, [FromBody] StationDto stationDto)
        {
            try
            {
                // Validate the stationDto object if the _validator is not null
                if (_validator != null)
                {
                    var validationResult = await _validator.ValidateAsync(stationDto);
                    if (!validationResult.IsValid)
                    {
                        var validationErrors = validationResult.Errors.Select(error => $"{error.PropertyName}: {error.ErrorMessage}");
                        var errorMessage = string.Join("\n", validationErrors);
                        return BadRequest(new { message = errorMessage });
                    }
                }
                await _stationService.UpdateStationAsync(id, stationDto);
                return Ok(new { message = "Station updated successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteStation(int id)
        {
            try
            {
                var result = await _stationService.DeleteStationAsync(id);

                if (result)
                {
                    return Ok(new { message = "Station deleted successfully." });
                }
                else
                {
                    return BadRequest(new { message = "Station not found." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
