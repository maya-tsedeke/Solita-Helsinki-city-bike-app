using AutoMapper;
using Backend.Applications.Interfaces.Repositories;
using Backend.Applications.Interfaces.Services;
using Backend.Domain.DTOs;
using Backend.Domain.Entities;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;


namespace Backend.Infrastructure.Services
{
    public class CSV_ImportService<T> : ICSV_ImportService<T> where T : JourneyDto
    {
        private readonly ICSV_Repository<T> _repository; 
        private readonly IMapper _mapper;

        public CSV_ImportService(ICSV_Repository<T> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<bool> ImportAsync(Stream stream)
        {
           
            var csvData = new List<T>();
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ",",
                TrimOptions = TrimOptions.Trim,
                IgnoreBlankLines = true,
                IgnoreReferences = true,
            };
           
     
            using (var reader = new StreamReader(stream))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                
                csvData = csv.GetRecords<T>().ToList();
                
            }

            var validatedData = ValidateData(csvData);

            var entities = _mapper.Map<List<T>>(validatedData);

            return await _repository.BulkInsertAsync(entities);
        }
        public async Task<bool> BulkInsertAsync(IEnumerable<T> entities)
        {
            return await _repository.BulkInsertAsync(entities);
        }

        private IEnumerable<T> ValidateData(IEnumerable<T> csvData)
        {
            var validatedData = csvData
                  .Where(j => TimeSpan.FromSeconds(j.DurationInSeconds) >= TimeSpan.FromSeconds(400) && j.CoveredDistanceInMeters >= 10)
                  .ToList();
      
            return validatedData;
        }
    }
}
