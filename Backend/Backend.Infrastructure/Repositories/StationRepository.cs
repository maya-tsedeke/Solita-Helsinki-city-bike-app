using Backend.Applications.Interfaces.Repositories;
using Backend.Domain.Entities;
using Backend.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Backend.Infrastructure.Repositories
{
    public class StationRepository : IStationRepository
    {
        private readonly AppDbcontext _dbContext;
        public StationRepository(AppDbcontext dbContext)
        {
            _dbContext = dbContext;
        }
        public IDbConnection Connection => _dbContext.Database.GetDbConnection();
        public async Task<List<Station>> GetStationsByIdsAsync(IEnumerable<string> ids)
        {
            return await _dbContext.Stations.Where(s => ids.Contains(s.ID.ToString())).ToListAsync();
        }

        public async Task<IEnumerable<Station>> ListStations(int limit, int offset, string orderBy, string search)
        {
            var query = _dbContext.Stations.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(s =>
                    s.Name.Contains(search) ||
                    s.Address.Contains(search)||
                    s.ID.ToString().Contains(search)
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

        public async Task<IEnumerable<Station>> GetAllAsync()
        {
            return await _dbContext.Stations.ToListAsync();
        }

        public async Task<Station> GetByIdAsync(int id)
        {
            return await _dbContext.Stations.FindAsync(id);
        }
        public async Task<bool> BulkInsert(DataTable table)
        {
            using (var connection = _dbContext.Database.GetDbConnection() as SqlConnection)
            {
                await connection.OpenAsync();

                using (var bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = "Stations";
                    // Set column mappings here
                    await bulkCopy.WriteToServerAsync(table);
                    return true;
                }
            }
        }
        public async Task<int> AddAsync(Station station)
        {
            _dbContext.Stations.Add(station);
            await _dbContext.SaveChangesAsync();
            return station.ID;
        }

        public async Task<bool> UpdateAsync(Station station)
        {
            _dbContext.Entry(station).State = EntityState.Modified;
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Station station)
        {
            var result = await GetByIdAsync(station.ID);
            if (result == null) return false;

            _dbContext.Stations.Remove(result);
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
