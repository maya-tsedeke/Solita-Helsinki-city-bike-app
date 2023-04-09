using Backend.Domain.DTOs;
using Backend.Domain.Entities;
using System.Data;

namespace Backend.Applications.Interfaces.Repositories
{
    public interface IJourneyRepository
    {
        Task<IEnumerable<Journey>> ListJourneys(int limit, int offset, string orderBy, string search);
        Task<bool> ImportJourneys(IEnumerable<Journey> journeys);
        Task<bool> BulkInsert(DataTable table);
        //Add journey for customer
        Task<IEnumerable<Station>> GetStations();
        Task<Station> GetStation(int stationId);
        Task AddJourney(Journey journey);
        //create departure info
        Task<Journey> GetJourneyById(int journeyId);
        //GetJourney by person
        Task<IEnumerable<Journey>> GetJourneysByUserId(int userId);
        //Update the return info
        Task UpdateJourney(Journey journey);
        //Single View 
        Task<IEnumerable<Journey>> GetAllAsync();
        Task<IEnumerable<Journey>> GetByMonthAsync(int month);
        Task<int> GetDepartureJourneyCountFromStationAsync(int stationId);
        Task<int> GetReturnJourneyCountToStationAsync(int stationId);
        Task<double> GetAverageDistanceOfDepartureJourneysFromStationAsync(int stationId);
        Task<double> GetAverageDistanceOfReturnJourneysToStationAsync(int stationId);
        Task<Dictionary<Station, int>> GetTop5ReturnStationsForStationAsync(int stationId);
        Task<Dictionary<Station, int>> GetTop5DepartureStationsForStationAsync(int stationId);

    }

}

