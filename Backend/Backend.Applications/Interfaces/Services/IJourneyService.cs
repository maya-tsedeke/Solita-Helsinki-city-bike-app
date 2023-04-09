using Backend.Domain.DTOs;
using Backend.Domain.Entities;


namespace Backend.Applications.Interfaces.Services
{
    public interface IJourneyService
    {
        Task<IEnumerable<JourneyDto>> ListJourneys(int limit = 100, int offset = 0, string orderBy = null, string search = null);
        Task<int> ImportJourneysFromCsv(string filePath);
        Task<JourneyDto> AddJourney(int stationId, DateTime departure, int userId); 
        Task<JourneyDto> UpdateJourneyReturnInfo(int journeyId, int returnStationId, DateTime returnDateTime);
        Task<IEnumerable<StationDto>> GetStations();
        //Get by Journey Id
        Task<JourneyDto> GetJourneys(int journeyId);
        //Get by UserId
        Task<IEnumerable<JourneyDto>> GetJourneysByUserId(int userId);
        //Single view
        Task<IEnumerable<Journey>> GetAllJourneysAsync();
        Task<IEnumerable<Journey>> GetJourneysByMonthAsync(int month);
        Task<int> GetDepartureJourneyCountFromStationAsync(int stationId);
        Task<int> GetReturnJourneyCountToStationAsync(int stationId);
        Task<double> GetAverageDistanceOfDepartureJourneysFromStationAsync(int stationId);
        Task<double> GetAverageDistanceOfReturnJourneysToStationAsync(int stationId);
        Task<Dictionary<Station, int>> GetTop5ReturnStationsForStationAsync(int stationId);
        Task<Dictionary<Station, int>> GetTop5DepartureStationsForStationAsync(int stationId);
      
    }
}
