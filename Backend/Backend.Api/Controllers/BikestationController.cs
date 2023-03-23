using AutoMapper;
using Backend.Applications.Interfaces.Repositories;
using Backend.Applications.Interfaces.Services;
using Backend.Domain.DTOs;
using Backend.Domain.Entities;
using Backend.Infrastructure.Repositories;
using Backend.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BikestationController : ControllerBase
    {
        private readonly IStationService _stationService; 


        public BikestationController(IStationService stationService) 
        {
            _stationService = stationService;
        }
        [HttpGet]
        public async Task<IEnumerable<StationDto>> ListStation(int limit = 100, int offset = 0, string orderBy = null, string search = null)
        {
            var journeys = await _stationService.ListStations(limit, offset, orderBy, search);
            return journeys;
        }
    }
}
