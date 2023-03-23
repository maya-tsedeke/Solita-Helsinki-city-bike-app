using Backend.Applications.Interfaces.Repositories;
using Backend.Domain.Entities;
using Backend.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;

namespace Backend.Infrastructure.Repositories
{
    public class StationRepository : IStationRepository
    {
        private readonly AppDbcontext _dbContext;
        public StationRepository(AppDbcontext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Station>> ListStations(int limit, int offset, string orderBy, string search)
        {
            var query = _dbContext.Stations.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(s =>
                    s.Name.Contains(search) ||
                    s.Address.Contains(search)
                );
            }

            if (!string.IsNullOrEmpty(orderBy))
            {
                switch (orderBy.ToLower())
                {
                    case "name":
                        query = query.OrderBy(s => s.Name);
                        break;
                    case "address":
                        query = query.OrderBy(s => s.Address);
                        break;
                    default:
                        query = query.OrderBy(s => s.ID);
                        break;
                }
            }

            query = query.Skip(offset).Take(limit);

            var stations = await query.ToListAsync();
            return stations;
        }

        public async Task<Station> GetStation(int stationId)
        {
            var station = await _dbContext.Stations.FindAsync(stationId);
            return station;
        }
        public async Task<bool> ImportJourneys(IEnumerable<Station> stations)
        {
            try
            {
                await _dbContext.Stations.AddRangeAsync(stations);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
