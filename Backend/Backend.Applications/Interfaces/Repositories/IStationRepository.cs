using Backend.Domain.Entities;

namespace Backend.Applications.Interfaces.Repositories
{
    public interface IStationRepository
    {
        Task<IEnumerable<Station>> ListStations(int limit, int offset, string orderBy, string search);
        Task<Station> GetStation(int stationId);
        Task<bool> ImportJourneys(IEnumerable<Station> stations);

     

    }
}
