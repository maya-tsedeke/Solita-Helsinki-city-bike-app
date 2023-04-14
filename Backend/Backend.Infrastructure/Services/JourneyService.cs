
using AutoMapper;
using Backend.Applications.Interfaces.Repositories;
using Backend.Applications.Interfaces.Services;
using Backend.Domain.DTOs;
using Backend.Domain.Entities;
using CsvHelper;
using OpenQA.Selenium;
using System.Data;
using System.Globalization;

namespace Backend.Infrastructure.Services
{
    public class JourneyService : IJourneyService
    {
        private readonly IJourneyRepository _journeyRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IStationRepository _stationRepository;
        public JourneyService(IJourneyRepository journeyRepository, IMapper mapper, IStationRepository stationRepository, IUserRepository userRepository)
        {
            _journeyRepository = journeyRepository;
            _mapper = mapper;
            _stationRepository = stationRepository;
            _userRepository = userRepository;
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
        public async Task<JourneyDto> GetJourneys(int journeyId)
        {
            var journey = await _journeyRepository.GetJourneyById(journeyId);

            if (journey == null)
            {
                throw new NotFoundException($"Journey with ID {journeyId} not found.");
            }
            var user = await _userRepository.GetByIdAsync(journeyId);

            if (journey == null)
            {
                throw new NotFoundException($"Journey with UserID {journeyId} not found.");
            }

            var departureStation = await _journeyRepository.GetStation(journey.DepartureStationId ?? 0);
            StationDto departureStationDto = null;
            if (departureStation != null)
            {
                departureStationDto = _mapper.Map<StationDto>(departureStation);
            }

            StationDto returnStationDto = null;
            if (journey.ReturnStationId.HasValue)
            {
                var returnStation = await _journeyRepository.GetStation(journey.ReturnStationId.Value);
                returnStationDto = _mapper.Map<StationDto>(returnStation);
            }
            UserDto userDto = null;
            if (journey.UserId.HasValue)
            {
                var userName = await _userRepository.GetByIdAsync(journey.UserId.Value);
                userDto = _mapper.Map<UserDto>(userName);
            }

            var response = new JourneyDto
            {
                Id = journey.Id,
                Departure = journey.Departure,
                Return = journey.Return,
                DepartureStationId = journey.DepartureStationId,
                ReturnStationId = journey.ReturnStationId.HasValue ? (int)journey.ReturnStationId : 0,
                CoveredDistanceInMeters = journey.CoveredDistanceInMeters,
                DurationInSeconds = journey.DurationInSeconds,
                DepartureStation = departureStationDto,
                ReturnStation = returnStationDto,
                users = userDto,
            };

            return response;
        }

        public async Task<IEnumerable<JourneyDto>> GetJourneysByUserId(int userId)
        {
            var journeys = await _journeyRepository.GetJourneysByUserId(userId);
            if (journeys == null || journeys.Count() == 0)
            {
                throw new NotFoundException($"Journeys with UserID {userId} not found.");
            }

            var journeyDtos = new List<JourneyDto>();
            foreach (var journey in journeys)
            {
                var departureStation = await _journeyRepository.GetStation(journey.DepartureStationId ?? 0);
                StationDto departureStationDto = null;
                if (departureStation != null)
                {
                    departureStationDto = _mapper.Map<StationDto>(departureStation);
                }

                StationDto returnStationDto = null;
                if (journey.ReturnStationId.HasValue)
                {
                    var returnStation = await _journeyRepository.GetStation(journey.ReturnStationId.Value);
                    returnStationDto = _mapper.Map<StationDto>(returnStation);
                }

                UserDto userDto = null;
                if (journey.UserId.HasValue)
                {
                    var userName = await _userRepository.GetByIdAsync(journey.UserId.Value);
                    userDto = _mapper.Map<UserDto>(userName);
                }

                var journeyDto = new JourneyDto
                {
                    Id = journey.Id,
                    Departure = journey.Departure,
                    Return = journey.Return,
                    DepartureStationId = journey.DepartureStationId,
                    ReturnStationId = journey.ReturnStationId.HasValue ? (int)journey.ReturnStationId : 0,
                    CoveredDistanceInMeters = journey.CoveredDistanceInMeters,
                    DurationInSeconds = journey.DurationInSeconds,
                    DepartureStation = departureStationDto,
                    ReturnStation = returnStationDto,
                    users = userDto
                };

                journeyDtos.Add(journeyDto);
            }

            return journeyDtos;
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
        //Add new Journy (Departure and Return)
        public async Task<IEnumerable<StationDto>> GetStations()
        {
            var stations = await _journeyRepository.GetStations();
            return stations.Select(s => new StationDto
            {
                ID = s.ID,
                Name = s.Name,
                x = s.x,
                y = s.y
            });
        }

        public async Task<JourneyDto> AddJourney(int stationId, DateTime departure, int userId)
        {
            var departureStation = await _journeyRepository.GetStation(stationId);
            if (departureStation == null)
            {
                throw new NotFoundException($"Departure station with ID {stationId} not found.");
            }
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                throw new NotFoundException($"Journey with UserID {userId} not found.");
            }
            var journey = _mapper.Map<Journey>(new JourneyDto { DepartureStationId = stationId, Departure = departure, UserId = userId, users = _mapper.Map<UserDto>(user) });
            journey.DepartureStation = departureStation;
            journey.ReturnStationId = null;
            journey.Return = DateTime.MinValue;
            journey.users = user;
            await _journeyRepository.AddJourney(journey);

            return _mapper.Map<JourneyDto>(journey);
        }

        public async Task<JourneyDto> UpdateJourneyReturnInfo(int journeyId, int returnStationId, DateTime returnDateTime)
        {
            var returnStation = await _journeyRepository.GetStation(returnStationId);
            if (returnStation == null)
            {
                throw new NotFoundException($"Return station with ID {returnStationId} not found.");
            }

            var journey = await _journeyRepository.GetJourneyById(journeyId);
            if (journey == null)
            {
                throw new NotFoundException($"Journey with ID {journeyId} not found.");
            }

            var departureStation = await _journeyRepository.GetStation(journey.DepartureStationId ?? 0); ;
            if (departureStation == null)
            {
                throw new NotFoundException($"Departure station with ID {journey.DepartureStationId} not found.");
            }

            var distance = CalculateDistance(departureStation.y, departureStation.x, returnStation.y, returnStation.x);
            if (journey.Departure.HasValue)
            {
                var duration = CalculateDurationInSeconds(journey.Departure.Value, returnDateTime);
                journey.DurationInSeconds = duration;
            }

            journey.ReturnStationId = returnStationId;
            journey.Return = returnDateTime;
            journey.CoveredDistanceInMeters = distance;

            await _journeyRepository.UpdateJourney(journey);

            return _mapper.Map<JourneyDto>(journey);
        }

        private double CalculateDurationInSeconds(DateTime start, DateTime end)
        {
            TimeSpan duration = end - start;
            return (double)duration.TotalSeconds;
        }

        private double CalculateDistance(double startX, double startY, double endX, double endY)
        {
            double distance = Math.Sqrt(Math.Pow(endX - startX, 2) + Math.Pow(endY - startY, 2));
            return distance;
        }


    }
}
