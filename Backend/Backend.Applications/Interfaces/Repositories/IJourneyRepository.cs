using Backend.Domain.Entities;
namespace Backend.Applications.Interfaces.Repositories
{
    public interface IJourneyRepository
    {
        Task<IEnumerable<Journey>> ListJourneys(int limit, int offset, string orderBy, string search);
        Task<bool> ImportJourneys(IEnumerable<Journey> journeys);
    }
}

