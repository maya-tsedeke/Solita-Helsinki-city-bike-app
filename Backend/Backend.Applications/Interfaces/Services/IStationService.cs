using Backend.Domain.DTOs;
using Backend.Domain.Entities;

namespace Backend.Applications.Interfaces.Services
{
    public interface IStationService
    {
        Task<IEnumerable<StationDto>> ListStations(int limit = 100, int offset = 0, string orderBy = null, string search = null);
        Task<StationDto> GetStation(int stationId);
        Task<bool> ImportJourneysFromCsv(string filePath);
     

    }

}
