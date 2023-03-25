using Backend.Domain.DTOs;
using Backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Applications.Interfaces.Services
{
    public interface IJourneyService
    {
        Task<IEnumerable<JourneyDto>> ListJourneys(int limit = 100, int offset = 0, string orderBy = null, string search = null);
        Task<int> ImportJourneysFromCsv(string filePath);
     

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
