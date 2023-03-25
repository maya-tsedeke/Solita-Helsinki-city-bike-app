using AutoMapper;
using Backend.Applications.Interfaces.Repositories;
using Backend.Domain.DTOs;
using Backend.Domain.Entities;
using Backend.Infrastructure.Persistence;
using CsvHelper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Globalization;

namespace Backend.Infrastructure.Repositories
{
    public class JourneyRepository : IJourneyRepository
    {
        private readonly AppDbcontext _dbContext;
        private readonly IMapper _mapper;
        public JourneyRepository(AppDbcontext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<bool> ImportJourneys(IEnumerable<Journey> journeys)
        {
            try
            {
                await _dbContext.Journeys.AddRangeAsync(journeys);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<Journey>> ListJourneys(int limit, int offset, string orderBy, string search)
        {
            var query = _dbContext.Journeys
                .Include(j => j.DepartureStation)
                .Include(j => j.ReturnStation)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(j =>
                    j.DepartureStation.Name.Contains(search) ||
                    j.ReturnStation.Name.Contains(search)
                );
            }

            if (!string.IsNullOrEmpty(orderBy))
            {
                switch (orderBy.ToLower())
                {
                    case "departure":
                        query = query.OrderBy(j => j.Departure);
                        break;
                    case "return":
                        query = query.OrderBy(j => j.Return);
                        break;
                    case "distance":
                        query = query.OrderBy(j => j.CoveredDistanceInMeters);
                        break;
                    case "duration":
                        query = query.OrderBy(j => j.DurationInSeconds);
                        break;
                    default:
                        query = query.OrderBy(j => j.Id);
                        break;
                }
            }

            query = query.Skip(offset).Take(limit);

            var journeys = await query.ToListAsync();
            return journeys;
        }
        public async Task<bool> BulkInsert(DataTable table)
        {
            using (var connection = _dbContext.Database.GetDbConnection() as SqlConnection)
            {
                await connection.OpenAsync();

                using (var bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = "Journeys";
            
                    // Set column mappings here
                    await bulkCopy.WriteToServerAsync(table);
                    return true;
                }
            }
        }


        //Single view
        public async Task<IEnumerable<Journey>> GetAllAsync()
        {
            return await _dbContext.Journeys
                .Include(j => j.DepartureStation)
                .Include(j => j.ReturnStation)
                .ToListAsync();
        }

        public async Task<IEnumerable<Journey>> GetByMonthAsync(int month)
        {
            return await _dbContext.Journeys
                .Where(j => j.Departure.Month == month || j.Return.Month == month)
                .Include(j => j.DepartureStation)
                .Include(j => j.ReturnStation)
                .ToListAsync();
        }


        public async Task<int> GetDepartureJourneyCountFromStationAsync(int stationId)
        {
            return await _dbContext.Journeys
                .CountAsync(j => j.DepartureStationId == stationId);
        }

        public async Task<int> GetReturnJourneyCountToStationAsync(int stationId)
        {
            return await _dbContext.Journeys
                .CountAsync(j => j.ReturnStationId == stationId);
        }

        public async Task<double> GetAverageDistanceOfDepartureJourneysFromStationAsync(int stationId)
        {
            return await _dbContext.Journeys
                .Where(j => j.DepartureStationId == stationId)
                .AverageAsync(j => j.CoveredDistanceInMeters);
        }

        public async Task<double> GetAverageDistanceOfReturnJourneysToStationAsync(int stationId)
        {
            return await _dbContext.Journeys
                .Where(j => j.ReturnStationId == stationId)
                .AverageAsync(j => j.CoveredDistanceInMeters);
        }

        public async Task<Dictionary<Station, int>> GetTop5ReturnStationsForStationAsync(int stationId)
        {
            return await _dbContext.Journeys
                .Where(j => j.DepartureStationId == stationId)
                .GroupBy(j => j.ReturnStation)
                .OrderByDescending(g => g.Count())
                .Take(5)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }

        public async Task<Dictionary<Station, int>> GetTop5DepartureStationsForStationAsync(int stationId)
        {
            return await _dbContext.Journeys
                .Where(j => j.ReturnStationId == stationId)
                .GroupBy(j => j.DepartureStation)
                .OrderByDescending(g => g.Count())
                .Take(5)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }

       
    }
}
