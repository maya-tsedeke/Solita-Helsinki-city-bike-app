﻿using AutoMapper;
using Backend.Applications.Interfaces.Repositories;
using Backend.Applications.Interfaces.Services;
using Backend.Domain.DTOs;
using Backend.Domain.Entities;
using Backend.Infrastructure.Repositories;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Net;

namespace Backend.Infrastructure.Services
{
    public class StationService : IStationService
    {
        private readonly IStationRepository _stationRepository;
        private readonly IMapper _mapper;
        private readonly IJourneyRepository _journeyRepository;
       

        public StationService(IStationRepository stationRepository, IMapper mapper,IJourneyRepository journeyRepository)
        {
            _stationRepository = stationRepository;
            _mapper = mapper;
            _journeyRepository= journeyRepository;
       
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
        public async Task<int> ImportStationFromCsv(string filePath)
        {
           
            var dataTable = new DataTable();
            dataTable.Columns.Add("FID", typeof(int));
            dataTable.Columns.Add("ID", typeof(int));
            dataTable.Columns.Add("Nimi", typeof(string));
            dataTable.Columns.Add("Namn", typeof(string));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Osoite", typeof(string));
            dataTable.Columns.Add("Address", typeof(string));
            dataTable.Columns.Add("Kaupunki", typeof(string));
            dataTable.Columns.Add("Stad", typeof(string));
            dataTable.Columns.Add("Operaattor", typeof(string));
            dataTable.Columns.Add("Kapasiteet", typeof(int));
            dataTable.Columns.Add("x", typeof(double));
            dataTable.Columns.Add("y", typeof(double));

            using (var reader = new StreamReader(filePath))
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HeaderValidated = null,
                    MissingFieldFound = null,
                    TrimOptions = TrimOptions.Trim,
                    IgnoreBlankLines = true,
                    HasHeaderRecord = true, // skip header record

                    BadDataFound = null,
                    
                };

                using (var csv = new CsvReader(reader, config))
                {
                    var records = csv.GetRecords<StationDto>();

                    foreach (var record in records)
                    {
                        if (record == null)
                        {
                            continue;
                        }
                        var recordArray = record.ToStringArray();

                        for (int i = 0; i < recordArray.Length; i++)
                        {
                            recordArray[i] = recordArray[i]?.Replace("'", "").Replace("\"", "");
                        }

                        dataTable.Rows.Add(recordArray);


                    }
                }
            }

            var result = await _stationRepository.BulkInsert(dataTable);

            return dataTable.Rows.Count ;
           
        }

        //Single station View 

        public async Task<IEnumerable<Station>> GetAllStationsAsync()
        {
            return await _stationRepository.GetAllAsync();
        }

        public async Task<Station> GetStationByIdAsync(int id)
        {
            return await _stationRepository.GetByIdAsync(id);
        }

        public async Task<string> GetStationNameAsync(int id)
        {
            var station = await _stationRepository.GetByIdAsync(id);
            return station.Name;
        }

        public async Task<string> GetStationAddressAsync(int id)
        {
            var station = await _stationRepository.GetByIdAsync(id);
            return station.Address;
        }

        public async Task<int> GetTotalDepartureJourneysFromStationAsync(int id)
        {
            return await _journeyRepository.GetDepartureJourneyCountFromStationAsync(id);
        }

        public async Task<int> GetTotalReturnJourneysToStationAsync(int id)
        {
            return await _journeyRepository.GetReturnJourneyCountToStationAsync(id);
        }

        public async Task<double[]> GetStationLocationAsync(int id)
        {
            var station = await _stationRepository.GetByIdAsync(id);
            return new double[] { station.x, station.y };
        }

        public async Task<double> GetAverageDistanceOfDepartureJourneysFromStationAsync(int id)
        {
            return await _journeyRepository.GetAverageDistanceOfDepartureJourneysFromStationAsync(id);
        }

        public async Task<double> GetAverageDistanceOfReturnJourneysToStationAsync(int id)
        {
            return await _journeyRepository.GetAverageDistanceOfReturnJourneysToStationAsync(id);
        }

        public async Task<IEnumerable<Journey>> GetJourneysByMonthAndStationAsync(int month, int stationId)
        {
            return await _journeyRepository.GetByMonthAsync(month);
        }
        public async Task<Dictionary<int, int>> GetTop5ReturnStationsForStationAsync(int stationId)
        {
            var journeys = await _journeyRepository.GetAllAsync();

            return await GetTop5StationsAsync(journeys.Where(j => j.DepartureStation.ID == stationId).AsQueryable(), "Return");
        }

        public async Task<Dictionary<int, int>> GetTop5DepartureStationsForStationAsync(int stationId)
        {
            var journeys = await _journeyRepository.GetAllAsync();

            return await GetTop5StationsAsync(journeys.Where(j => j.ReturnStation.ID == stationId).AsQueryable(), "Departure");
        }

        private async Task<Dictionary<int, int>> GetTop5StationsAsync(IQueryable<Journey> journeys, string type)
        {
            var topStations = await journeys
                .GroupBy(j => type == "Return" ? j.ReturnStation.ID : j.DepartureStation.ID)
                .OrderByDescending(g => g.Count())
                .Take(5)
                .ToListAsync();

            var results = new Dictionary<int, int>();

            foreach (var stationGroup in topStations)
            {
                results.Add(stationGroup.Key, stationGroup.Count());
            }

            return results;
        }

    }
}
