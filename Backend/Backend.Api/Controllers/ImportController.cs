using AutoMapper;
using Backend.Applications.Interfaces.Services;
using Backend.Domain.DTOs;
using Backend.Domain.Entities;
using Backend.Domain.Request;
using Backend.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using static System.Collections.Specialized.BitVector32;

namespace Backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportController : ControllerBase
    {
        private readonly IStationService _stationService;
        private readonly IJourneyService _journeyService;

        public ImportController(IStationService stationService, IJourneyService journeyService)
        {
            _stationService = stationService;
            _journeyService = journeyService;
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

            try
            {
                var filePath = Path.GetTempFileName();
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var result = await _journeyService.ImportJourneysFromCsv(filePath);

                if (result)
                {
                    return Ok();
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Failed to import journeys.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Failed to import journeys: {ex.Message}");
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

                var result = await _stationService.ImportJourneysFromCsv(filePath);
            try
            {
                if (result)
                {
                    return StatusCode(StatusCodes.Status200OK, $"Success to import journeys.{result}");
                    // return Ok($"Successfully imported. {result}");
                }
                else
                {
                 
                    return StatusCode(StatusCodes.Status500InternalServerError, $"Failed to import journeys.{result}");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Failed to import journeys: {ex.Message}");
            }
        } 
    }
}
