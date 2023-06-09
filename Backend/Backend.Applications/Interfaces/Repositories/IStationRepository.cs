﻿using Backend.Domain.Entities;
using System.Data;

namespace Backend.Applications.Interfaces.Repositories
{
    public interface IStationRepository
    {
        Task<List<Station>> GetStationsByIdsAsync(IEnumerable<string> ids);
        Task<IEnumerable<Station>> ListStations(int limit, int offset, string orderBy, string search);
        Task<Station> GetStation(int stationId);
        Task<bool> ImportJourneys(IEnumerable<Station> stations);
        Task<bool> BulkInsert(DataTable table);
        Task<int> AddAsync(Station station);
        Task<bool> UpdateAsync(Station station);
        Task<bool> DeleteAsync(Station station);
        //Single view
        Task<IEnumerable<Station>> GetAllAsync();
        Task<Station> GetByIdAsync(int id);


    }
}
