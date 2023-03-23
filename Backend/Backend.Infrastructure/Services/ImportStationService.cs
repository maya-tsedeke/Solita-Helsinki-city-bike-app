using AutoMapper;
using Backend.Applications.Interfaces.Repositories;
using Backend.Applications.Interfaces.Services;
using Backend.Domain.Entities;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;


namespace Backend.Infrastructure.Services
{
    public class ImportStationService<T> : IImportStationService<T> where T : Station
    {
        private readonly IImportStationRepository<T> _repository; 
        private readonly IMapper _mapper;

        public ImportStationService(IImportStationRepository<T> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<bool> ImportAsync(Stream stream)
        {
            var csvData = new List<T>();
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
             
                Delimiter = ",",
          
            };
            using (var reader = new StreamReader(stream))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                
                csvData = csv.GetRecords<T>().ToList();
                
            }
            var entities = _mapper.Map<List<T>>(csvData);

            return await _repository.BulkInsertAsync(entities);
        }
        public async Task<bool> BulkInsertAsync(IEnumerable<T> entities)
        {
            return await _repository.BulkInsertAsync(entities);
        }


    }
}
