using AutoMapper;
using Backend.Applications.Interfaces.Repositories;
using Backend.Applications.Interfaces.Services;
using Backend.Domain.DTOs;
using Backend.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Collections.Specialized.BitVector32;

namespace Backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StationController : ControllerBase
    {
        private readonly IStationService _stationService;

        public StationController(IStationService stationService)
        {
            _stationService = stationService;
        }
        [HttpPost("Create")]
        [Authorize]
        public async Task<IActionResult> CreateStation([FromBody] StationDto stationDto)
        {
            try
            {
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
