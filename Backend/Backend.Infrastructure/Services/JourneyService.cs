
using AutoMapper;
using Backend.Applications.Interfaces.Repositories;
using Backend.Applications.Interfaces.Services;
using Backend.Domain.DTOs;
using Backend.Domain.Entities;
using Backend.Domain.Validation;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System.Data;
using System.Globalization;

namespace Backend.Infrastructure.Services
{
    public class JourneyService : IJourneyService
    {
        private readonly IJourneyRepository _journeyRepository;
        private readonly IMapper _mapper;
        private readonly IStationRepository _stationRepository;
        public JourneyService(IJourneyRepository journeyRepository, IMapper mapper, IStationRepository stationRepository)
        {
            _journeyRepository = journeyRepository;
            _mapper = mapper;
            _stationRepository = stationRepository;
        }
        
        public async Task<int> ImportJourneysFromCsv(string filePath)
        {

            var journeys = new List<Journey>();
            int counter = 0;
            using (var reader = new StreamReader(filePath))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {

                    var records = csv.GetRecords<CSVDto>();
                    
                    foreach (var record in records)
                    {

                        var departureStation = await _stationRepository.GetStation(record.DepartureStationId);
                        var returnStation = await _stationRepository.GetStation(record.ReturnStationId);

                        if (departureStation != null && returnStation != null)
                        {



                            var journey = new Journey
                            {
                                Departure = record.Departure,
                                Return = record.Return,
                                DepartureStationId = record.DepartureStationId,
                                ReturnStationId = record.ReturnStationId,
                                CoveredDistanceInMeters = record.CoveredDistanceInMeters,
                                DurationInSeconds = record.DurationInSeconds,
                            };

                            if (journey.DurationInSeconds >= 10 && journey.CoveredDistanceInMeters >= 10)
                            {
                                journeys.Add(journey);
                                counter++;
                            }
                        }
                    }
                }
            }
            await _journeyRepository.ImportJourneys(journeys);
            return counter;
        }

        public async Task<IEnumerable<JourneyDto>> ListJourneys(int limit = 100, int offset = 0, string orderBy = null, string search = null)
        {
            var journeys = await _journeyRepository.ListJourneys(limit, offset, orderBy, search);
            return _mapper.Map<IEnumerable<JourneyDto>>(journeys);
        }

        //Single station
        public async Task<IEnumerable<Journey>> GetAllJourneysAsync()
        {
            return await _journeyRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Journey>> GetJourneysByMonthAsync(int month)
        {
            return await _journeyRepository.GetByMonthAsync(month);
        }

        public async Task<int> GetDepartureJourneyCountFromStationAsync(int stationId)
        {
            return await _journeyRepository.GetDepartureJourneyCountFromStationAsync(stationId);
        }

        public async Task<int> GetReturnJourneyCountToStationAsync(int stationId)
        {
            return await _journeyRepository.GetReturnJourneyCountToStationAsync(stationId);
        }

        public async Task<double> GetAverageDistanceOfDepartureJourneysFromStationAsync(int stationId)
        {
            return await _journeyRepository.GetAverageDistanceOfDepartureJourneysFromStationAsync(stationId);
        }

        public async Task<double> GetAverageDistanceOfReturnJourneysToStationAsync(int stationId)
        {
            return await _journeyRepository.GetAverageDistanceOfReturnJourneysToStationAsync(stationId);
        }

        public async Task<Dictionary<Station, int>> GetTop5ReturnStationsForStationAsync(int stationId)
        {
            return await _journeyRepository.GetTop5ReturnStationsForStationAsync(stationId);
        }

        public async Task<Dictionary<Station, int>> GetTop5DepartureStationsForStationAsync(int stationId)
        {
            return await _journeyRepository.GetTop5DepartureStationsForStationAsync(stationId);
        }

    }
}
