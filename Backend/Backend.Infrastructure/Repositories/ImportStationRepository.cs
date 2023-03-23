using Backend.Applications.Interfaces.Repositories;
using Backend.Domain.DTOs;
using Backend.Domain.Entities;
using Backend.Infrastructure.Persistence;
using System;

namespace Backend.Infrastructure.Repositories
{//By adding the where T : class constraint, you are telling the compiler that T must be a reference type.
    
    public class ImportStationRepository<T> : IImportStationRepository<T> where T : Station
    {
        private readonly AppDbcontext _dbContext;
        public ImportStationRepository(AppDbcontext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> BulkInsertAsync(IEnumerable<T> entities)
        {
            await _dbContext.Set<T>().AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public IQueryable<T> GetAllJourneys()
        {
            return _dbContext.Set<T>();
        }
    }
}
