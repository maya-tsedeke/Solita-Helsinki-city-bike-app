using AutoMapper;
using Backend.Applications.Interfaces.Repositories;
using Backend.Domain.Entities;
using Backend.Infrastructure.Persistence;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;

namespace Backend.Infrastructure.Repositories
{
    public class JourneyRepository : IJourneyRepository
    {
        private readonly AppDbcontext _dbContext;

        public JourneyRepository(AppDbcontext dbContext)
        {
            _dbContext = dbContext;
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

    

    }
}
