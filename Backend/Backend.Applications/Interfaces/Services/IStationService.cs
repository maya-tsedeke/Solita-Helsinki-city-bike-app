using Backend.Domain.DTOs;
using Backend.Domain.Entities;

namespace Backend.Applications.Interfaces.Services
{
    public interface IStationService
    {
        Task<IEnumerable<StationDto>> ListStations(int limit = 100, int offset = 0, string orderBy = null, string search = null);
        Task<StationDto> GetStation(int stationId);
        Task<int> ImportStationFromCsv(string filePath); 


        //Single View
        Task<IEnumerable<Station>> GetAllStationsAsync();
        Task<Station> GetStationByIdAsync(int id);
        Task<string> GetStationNameAsync(int id);
        Task<string> GetStationAddressAsync(int id);
        Task<int> GetTotalDepartureJourneysFromStationAsync(int id);
        Task<int> GetTotalReturnJourneysToStationAsync(int id);
        Task<double[]> GetStationLocationAsync(int id);
        Task<double> GetAverageDistanceOfDepartureJourneysFromStationAsync(int id);
        Task<double> GetAverageDistanceOfReturnJourneysToStationAsync(int id);
        Task<Dictionary<int, int>> GetTop5ReturnStationsForStationAsync(int id);
        Task<Dictionary<int, int>> GetTop5DepartureStationsForStationAsync(int id);
        Task<IEnumerable<Journey>> GetJourneysByMonthAndStationAsync(int month, int stationId);



    }

}
