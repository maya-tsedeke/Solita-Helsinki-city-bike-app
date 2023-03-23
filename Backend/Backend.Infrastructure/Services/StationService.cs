using AutoMapper;
using Backend.Applications.Interfaces.Repositories;
using Backend.Applications.Interfaces.Services;
using Backend.Domain.DTOs;
using Backend.Domain.Entities;
using Backend.Infrastructure.Repositories;
using CsvHelper;
using System.Globalization;

namespace Backend.Infrastructure.Services
{
    public class StationService : IStationService
    {
        private readonly IStationRepository _stationRepository;
        private readonly IMapper _mapper;

        public StationService(IStationRepository stationRepository, IMapper mapper)
        {
            _stationRepository = stationRepository;
            _mapper = mapper;
        }
  
        public async Task<IEnumerable<StationDto>> ListStations(int limit = 100, int offset = 0, string orderBy = null, string search = null)
        {
            var stations = await _stationRepository.ListStations(limit, offset, orderBy, search);
            return _mapper.Map<IEnumerable<StationDto>>(stations);
        }

        public async Task<StationDto> GetStation(int stationId)
        {
            var station = await _stationRepository.GetStation(stationId);
            return _mapper.Map<StationDto>(station);
        }
        public async Task<bool> ImportJourneysFromCsv(string filePath)
        {
            var stations = new List<Station>();
            try { 
        
            using (var reader = new StreamReader(filePath))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {

                    var records = csv.GetRecords<StationDto>();

                    foreach (var record in records)
                    {
                        // var departureStation = await _stationRepository.GetStation(record.DepartureStationId);
                        // var returnStation = await _stationRepository.GetStation(record.ReturnStationId);

                        //if (departureStation == null || returnStation == null)
                        //{
                        //    continue;
                        //}

                        var station = new Station
                        {
                            FID = record.FID,
                            ID = record.ID,
                            Nimi = record.Nimi,//departureStation.ID,
                            Namn = record.Namn,//returnStation.ID,
                            Name = record.Name,
                            Osoite = record.Osoite,
                            Address= record.Address,
                            Kaupunki= record.Kaupunki,
                            Stad= record.Stad,
                            Operaattor= record.Operaattor,
                            Kapasiteet= record.Kapasiteet,
                            x=record.x,
                            y=record.y,
                        };
                            // log the exception here
                          
                        
                            stations.Add(station);
                        
                    }
                }
            }

            return await _stationRepository.ImportJourneys(stations);
            }
            catch (Exception ex)
            {
                // log the exception here
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
    }
}
