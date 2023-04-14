using AutoMapper;
using Backend.Applications.Interfaces.Repositories;
using Backend.Domain.Entities;
using Backend.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
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
        public async Task<IEnumerable<Station>> GetStations()
        {
            return await _dbContext.Stations.ToListAsync();
        }

        public async Task<Station> GetStation(int stationId)
        {
            return await _dbContext.Stations.FindAsync(stationId);
        }
        //Get by Journey ID
        public async Task<Journey> GetJourneyById(int journyId)
        {
            var journey = await _dbContext.Journeys.FindAsync(journyId);
            if (journey != null)
            {
                journey.users = await _dbContext.Users.FindAsync(journey.UserId);
            }
            return journey;
        }
        //Get by userName
        public async Task<IEnumerable<Journey>> GetJourneysByUserId(int userId)
        {
            return await _dbContext.Journeys
                .Where(j => j.UserId == userId)
                .ToListAsync();
        }
        public async Task AddJourney(Journey journey)
        {
            _dbContext.Journeys.Add(journey);

            try
            {
                // Call a method that saves changes to the database
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Get the inner exception if there is one
                Exception innerEx = ex.InnerException;

                // Loop through inner exceptions to get the root cause of the error
                while (innerEx != null && innerEx.InnerException != null)
                {
                    innerEx = innerEx.InnerException;
                }

                // Log or handle the error
                Console.WriteLine($"Error: {innerEx?.Message}");
            }
        }
        public async Task UpdateJourney(Journey journey)
        {
            _dbContext.Entry(journey).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
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
                .Where(j => (j.Departure.HasValue && j.Departure.Value.Month == month) || (j.Return.HasValue && j.Return.Value.Month == month))
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
            var average_d = await _dbContext.Journeys
                .Where(j => j.DepartureStationId == stationId)
                .AverageAsync(j => j.CoveredDistanceInMeters);
            return average_d ?? 0; // if average is null, return 0 
        }

        public async Task<double> GetAverageDistanceOfReturnJourneysToStationAsync(int stationId)
        {
            var average_r = await _dbContext.Journeys
                .Where(j => j.ReturnStationId == stationId)
                .AverageAsync(j => j.CoveredDistanceInMeters);
            return average_r ?? 0;
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
