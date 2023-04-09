using AutoMapper;
using Backend.Applications.Interfaces.Repositories;
using Backend.Applications.Interfaces.Services;
using Backend.Domain.DTOs;
using Backend.Domain.Entities;
using Backend.Infrastructure.Repositories;
using Backend.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;

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
        [Authorize]
        public async Task<IEnumerable<JourneyDto>> ListJourneys(int limit = 100, int offset = 0, string orderBy = null, string search = null)
        {
            var journeys = await _journeyService.ListJourneys(limit, offset, orderBy, search);
            return journeys;
        }
        [HttpPost("Add")]
        [Authorize]
        public async Task<IActionResult> AddJourney([FromBody] CreateJourneyDepartureDto request)
        {
            try
            {
                var addedJourney = await _journeyService.AddJourney(request.DepartureStationId,request.DepartureDateTime,request.UserId);

                var response = new JourneyDto
                {
                    Id = addedJourney.Id,
                    Departure = addedJourney.Departure,
                    Return = addedJourney.Return,
                    DepartureStationId = addedJourney.DepartureStationId,
                    ReturnStationId = addedJourney.ReturnStationId,
                    CoveredDistanceInMeters = addedJourney.CoveredDistanceInMeters,
                    DurationInSeconds = addedJourney.DurationInSeconds,
                    UserId = addedJourney.UserId,
                    DepartureStation = new StationDto
                    {
                        FID = addedJourney.DepartureStation.FID,
                        ID = addedJourney.DepartureStation.ID,
                        Name = addedJourney.DepartureStation.Name,
                        x = addedJourney.DepartureStation.x,
                        y = addedJourney.DepartureStation.y,

                        Namn = addedJourney.DepartureStation.Namn,
                        Nimi = addedJourney.DepartureStation.Nimi,
                        Operaattor = addedJourney.DepartureStation.Operaattor,
                        Kapasiteet = addedJourney.DepartureStation.Kapasiteet,
                        Osoite = addedJourney.DepartureStation.Osoite,
                        Address = addedJourney.DepartureStation.Address,
                        Kaupunki = addedJourney.DepartureStation.Kaupunki,
                        Stad = addedJourney.DepartureStation.Stad,
                    },
                    ReturnStation = addedJourney.ReturnStation == null ? null : new StationDto
                    {
               
                        FID = addedJourney.ReturnStation.FID,
                        ID = addedJourney.ReturnStation.ID,
                        Name = addedJourney.ReturnStation.Name,
                        x = addedJourney.ReturnStation.x,
                        y = addedJourney.ReturnStation.y,

                        Namn = addedJourney.ReturnStation.Namn,
                        Nimi = addedJourney.ReturnStation.Nimi,
                        Operaattor = addedJourney.ReturnStation.Operaattor,
                        Kapasiteet = addedJourney.ReturnStation.Kapasiteet,
                        Osoite = addedJourney.ReturnStation.Osoite,
                        Address = addedJourney.ReturnStation.Address,
                        Stad = addedJourney.ReturnStation.Stad,
                        Kaupunki = addedJourney.ReturnStation.Kaupunki
                    },
                    users = addedJourney.users == null ? null : new UserDto
                    {
                        Id = addedJourney.users.Id,
                        Firstname = addedJourney.users.Firstname,
                        Lastname = addedJourney.users.Lastname,
                        Username = addedJourney.users.Username,
                        Token = addedJourney.users.Token,
                        role = addedJourney.users.role,
                        Email = addedJourney.users.Email,

                    }
                };

                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Handle error
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPut("{journeyId:int}/return")]
        [Authorize]
        public async Task<IActionResult> UpdateJourneyReturnInfo(int journeyId, [FromBody] UpdateJourneyReturnDto request)
        {
            try
            {
                var updatedJourney = await _journeyService.UpdateJourneyReturnInfo(journeyId, request.ReturnStationId, request.ReturnDateTime);

                var response = new JourneyDto
                {
                    Id = updatedJourney.Id,
                    Departure = updatedJourney.Departure,
                    Return = updatedJourney.Return,
                    DepartureStationId = updatedJourney.DepartureStationId,
                    ReturnStationId = updatedJourney.ReturnStationId,
                    CoveredDistanceInMeters = updatedJourney.CoveredDistanceInMeters,
                    DurationInSeconds = updatedJourney.DurationInSeconds,
                    UserId= updatedJourney.UserId,
                    DepartureStation = new StationDto
                    {
                        FID = updatedJourney.DepartureStation.FID,
                        ID = updatedJourney.DepartureStation.ID,
                        Name = updatedJourney.DepartureStation.Name,
                        x = updatedJourney.DepartureStation.x,
                        y = updatedJourney.DepartureStation.y,

                        Namn = updatedJourney.DepartureStation.Namn,
                        Nimi = updatedJourney.DepartureStation.Nimi,
                        Operaattor = updatedJourney.DepartureStation.Operaattor,
                        Kapasiteet = updatedJourney.DepartureStation.Kapasiteet,
                        Osoite = updatedJourney.DepartureStation.Osoite,
                        Address = updatedJourney.DepartureStation.Address,
                        Stad = updatedJourney.DepartureStation.Stad,
                        Kaupunki = updatedJourney.DepartureStation.Kaupunki
                    },
                    ReturnStation = new StationDto
                    {
                        FID = updatedJourney.ReturnStation.FID,
                        ID = updatedJourney.ReturnStation.ID,
                        Name = updatedJourney.ReturnStation.Name,
                        x = updatedJourney.ReturnStation.x,
                        y = updatedJourney.ReturnStation.y,

                        Namn = updatedJourney.ReturnStation.Namn,
                        Nimi = updatedJourney.ReturnStation.Nimi,
                        Operaattor = updatedJourney.ReturnStation.Operaattor,
                        Kapasiteet = updatedJourney.ReturnStation.Kapasiteet,
                        Osoite = updatedJourney.ReturnStation.Osoite,
                        Address = updatedJourney.ReturnStation.Address,
                        Stad = updatedJourney.ReturnStation.Stad,
                        Kaupunki = updatedJourney.ReturnStation.Kaupunki
                    },
                    users = updatedJourney.users=new UserDto
                    {
                        Id = updatedJourney.users.Id,
                        Firstname = updatedJourney.users.Firstname,
                        Lastname = updatedJourney.users.Lastname,
                        Username = updatedJourney.users.Username,
                        Token = updatedJourney.users.Token,
                        role = updatedJourney.users.role,
                        Email = updatedJourney.users.Email,

                    }
                };

                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Handle error
                return StatusCode(500, ex.Message);
            }
        }

        public class UpdateJourneyReturnDto
        {
            public int ReturnStationId { get; set; }
            public DateTime ReturnDateTime { get; set; }
        }
        public class CreateJourneyDepartureDto 
        {
            public int DepartureStationId { get; set; }
            public DateTime DepartureDateTime { get; set; } = DateTime.Now;
            public int UserId { get; set; }
        }
        [HttpGet("{journId}")]
        [Authorize]
        public async Task<IActionResult> GetTripByJourneyId(int journId)
        {
            try
            {
                var journeyDetail = await _journeyService.GetJourneys(journId);

                var response = new JourneyDto
                {
                    Id = journeyDetail.Id,
                    Departure = journeyDetail.Departure,
                    Return = journeyDetail.Return,
                    DepartureStationId = journeyDetail.DepartureStationId , 
                    ReturnStationId = journeyDetail.ReturnStationId,
                    CoveredDistanceInMeters = journeyDetail.CoveredDistanceInMeters,
                    DurationInSeconds = journeyDetail.DurationInSeconds,
                    UserId= journeyDetail.UserId,

                    DepartureStation = journeyDetail.DepartureStation != null ? new StationDto
                    {
                        FID = journeyDetail.DepartureStation.FID,
                        ID = journeyDetail.DepartureStation.ID,
                        Name = journeyDetail.DepartureStation.Name,
                        x = journeyDetail.DepartureStation.x,
                        y = journeyDetail.DepartureStation.y,
                        Namn = journeyDetail.DepartureStation.Namn,
                        Nimi = journeyDetail.DepartureStation.Nimi,
                        Operaattor = journeyDetail.DepartureStation.Operaattor,
                        Kapasiteet = journeyDetail.DepartureStation.Kapasiteet,
                        Osoite = journeyDetail.DepartureStation.Osoite,
                        Address = journeyDetail.DepartureStation.Address,
                        Stad = journeyDetail.DepartureStation.Stad,
                        Kaupunki = journeyDetail.DepartureStation.Kaupunki
                    } : null,
                    ReturnStation = journeyDetail.ReturnStation != null ? new StationDto
                    {
                        FID = journeyDetail.ReturnStation.FID,
                        ID = journeyDetail.ReturnStation.ID,
                        Name = journeyDetail.ReturnStation.Name,
                        x = journeyDetail.ReturnStation.x,
                        y = journeyDetail.ReturnStation.y,
                        Namn = journeyDetail.ReturnStation.Namn,
                        Nimi = journeyDetail.ReturnStation.Nimi,
                        Operaattor = journeyDetail.ReturnStation.Operaattor,
                        Kapasiteet = journeyDetail.ReturnStation.Kapasiteet,
                        Osoite = journeyDetail.ReturnStation.Osoite,
                        Address = journeyDetail.ReturnStation.Address,
                        Stad = journeyDetail.ReturnStation.Stad,
                        Kaupunki = journeyDetail.ReturnStation.Kaupunki
                    } : null,
                       users= journeyDetail.users != null ? new UserDto
                      {
                        Id = journeyDetail.users.Id,
                        Firstname = journeyDetail.users.Firstname,
                        Lastname = journeyDetail.users.Lastname,
                        Username = journeyDetail.users.Username,
                        Token = journeyDetail.users.Token,
                        role = journeyDetail.users.role,
                        Email = journeyDetail.users.Email,

                       } : null
                };

                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet("{userId:int}/User")]
        [Authorize]
        public async Task<IEnumerable<JourneyDto>> GetJourneysByUserId(int userId)
        {
            try
            {
                var journeyDetails = await _journeyService.GetJourneysByUserId(userId);
                var responseList = new List<JourneyDto>();

                foreach (var journeyDetail in journeyDetails)
                {
                    var response = new JourneyDto
                    {
                        Id = journeyDetail.Id,
                        Departure = journeyDetail.Departure,
                        Return = journeyDetail.Return,
                        DepartureStationId = journeyDetail.DepartureStationId,
                        ReturnStationId = journeyDetail.ReturnStationId,
                        CoveredDistanceInMeters = journeyDetail.CoveredDistanceInMeters,
                        DurationInSeconds = journeyDetail.DurationInSeconds,
                        UserId = journeyDetail.UserId,

                        DepartureStation = journeyDetail.DepartureStation != null ? new StationDto
                        {
                            FID = journeyDetail.DepartureStation.FID,
                            ID = journeyDetail.DepartureStation.ID,
                            Name = journeyDetail.DepartureStation.Name,
                            x = journeyDetail.DepartureStation.x,
                            y = journeyDetail.DepartureStation.y,
                            Namn = journeyDetail.DepartureStation.Namn,
                            Nimi = journeyDetail.DepartureStation.Nimi,
                            Operaattor = journeyDetail.DepartureStation.Operaattor,
                            Kapasiteet = journeyDetail.DepartureStation.Kapasiteet,
                            Osoite = journeyDetail.DepartureStation.Osoite,
                            Address = journeyDetail.DepartureStation.Address,
                            Stad = journeyDetail.DepartureStation.Stad,
                            Kaupunki = journeyDetail.DepartureStation.Kaupunki
                        } : null,
                        ReturnStation = journeyDetail.ReturnStation != null ? new StationDto
                        {
                            FID = journeyDetail.ReturnStation.FID,
                            ID = journeyDetail.ReturnStation.ID,
                            Name = journeyDetail.ReturnStation.Name,
                            x = journeyDetail.ReturnStation.x,
                            y = journeyDetail.ReturnStation.y,
                            Namn = journeyDetail.ReturnStation.Namn,
                            Nimi = journeyDetail.ReturnStation.Nimi,
                            Operaattor = journeyDetail.ReturnStation.Operaattor,
                            Kapasiteet = journeyDetail.ReturnStation.Kapasiteet,
                            Osoite = journeyDetail.ReturnStation.Osoite,
                            Address = journeyDetail.ReturnStation.Address,
                            Stad = journeyDetail.ReturnStation.Stad,
                            Kaupunki = journeyDetail.ReturnStation.Kaupunki
                        } : null,
                        users = journeyDetail.users != null ? new UserDto
                        {
                            Id = journeyDetail.users.Id,
                            Firstname = journeyDetail.users.Firstname,
                            Lastname = journeyDetail.users.Lastname,
                            Username = journeyDetail.users.Username,
                            Token = journeyDetail.users.Token,
                            role = journeyDetail.users.role,
                            Email = journeyDetail.users.Email,

                        } : null
                    };

                    responseList.Add(response);
                }

                return responseList;
            }
            catch (NotFoundException)
            {

                return Enumerable.Empty<JourneyDto>();
            }
            catch (Exception)
            {
                return Enumerable.Empty<JourneyDto>();
            }
        }

    }
}
