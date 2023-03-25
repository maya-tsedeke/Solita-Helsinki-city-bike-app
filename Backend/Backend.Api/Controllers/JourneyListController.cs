using AutoMapper;
using Backend.Applications.Interfaces.Repositories;
using Backend.Applications.Interfaces.Services;
using Backend.Domain.DTOs;
using Backend.Domain.Entities;
using Backend.Infrastructure.Repositories;
using Backend.Infrastructure.Services;

using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JourneyListController : ControllerBase
    {
        private readonly IJourneyService _journeyService;
     

        public JourneyListController(IJourneyService journeyService)
        {
            _journeyService = journeyService;
          
        }
        [HttpGet]
        public async Task<IEnumerable<JourneyDto>> ListJourneys(int limit = 100, int offset = 0, string orderBy = null, string search = null)
        {
            var journeys = await _journeyService.ListJourneys(limit, offset, orderBy, search);
            return journeys;
        }
        
    }
}
