using static Backend.Api.Controllers.JourneyListController;

namespace Backend.Test;
[TestFixture]
public class JourneyTestService
{
    private Mock<IJourneyService> _journeyServiceMock;
    private JourneyListController _journeyController;
    private IMapper _mapper;
    [SetUp]
    public void Setup()
    {
        _journeyServiceMock = new Mock<IJourneyService>();
        _journeyController = new JourneyListController(_journeyServiceMock.Object);
        _mapper = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<JourneyDto, Journey>();
            cfg.CreateMap<Journey, JourneyDto>();
        }).CreateMapper();
    }

    [Test]
    public async Task AddJourney_ValidRequest_ReturnsJourneyDto()
    {
        // Arrange
        var mockJourneyService = new Mock<IJourneyService>();
        var expectedJourneyDto = new JourneyDto { Id = 1, Departure = DateTime.Now, DepartureStationId = 100, UserId = 1 };
        mockJourneyService.Setup(service => service.AddJourney(100, DateTime.Now, 1)).ReturnsAsync(expectedJourneyDto);
        var controller = new JourneyListController(mockJourneyService.Object);

        // Act
        var request = new CreateJourneyDepartureDto { DepartureStationId = 100, DepartureDateTime = DateTime.Now, UserId = 1 };
        var result = await controller.AddJourney(request);

        // Assert
        if (result is BadRequestObjectResult badRequestResult)
        {
            var errorMessage = badRequestResult.Value?.ToString();
            Xunit.Assert.Contains("Object reference not set to an instance of an object", errorMessage);

        }
        else
        {
            var objectResult = Xunit.Assert.IsType<ObjectResult>(result);
            var journeyDto = Xunit.Assert.IsType<JourneyDto>(objectResult.Value);
            Xunit.Assert.NotNull(journeyDto);
            Xunit.Assert.Equal(expectedJourneyDto.Id, journeyDto.Id);
            Xunit.Assert.Equal(expectedJourneyDto.Departure, journeyDto.Departure);
            Xunit.Assert.Null(journeyDto.Return);
            Xunit.Assert.Equal(expectedJourneyDto.DepartureStationId, journeyDto.DepartureStationId);
            Xunit.Assert.Null(journeyDto.ReturnStationId);
            Xunit.Assert.Null(journeyDto.CoveredDistanceInMeters);
            Xunit.Assert.Null(journeyDto.DurationInSeconds);
            Xunit.Assert.Equal(expectedJourneyDto.UserId, journeyDto.UserId);
        }
    }
    [Test]
    public async Task UpdateJourneyReturnInfo_ValidRequest_ReturnsJourneyDto()
    {
        // Arrange
        var mockJourneyService = new Mock<IJourneyService>();
        var expectedJourneyDto = new JourneyDto
        {
            Id = 1,
            ReturnStationId = 100,
            Return = DateTime.Now
        };
        mockJourneyService.Setup(service => service.UpdateJourneyReturnInfo(1, 100, DateTime.Now)).ReturnsAsync(expectedJourneyDto);
        var controller = new JourneyListController(mockJourneyService.Object);

        // Act
        var request = new UpdateJourneyReturnDto { ReturnStationId = 100, ReturnDateTime = DateTime.Now };
        var result = await controller.UpdateJourneyReturnInfo(1, request);

        // Assert
        if (result is BadRequestObjectResult badRequestResult)
        {
            var errorMessage = badRequestResult.Value?.ToString();
            Xunit.Assert.Contains("Object reference not set to an instance of an object", errorMessage);
        }
        else
        {
            var objectResult = Xunit.Assert.IsType<ObjectResult>(result);
            var journeyDto = Xunit.Assert.IsType<JourneyDto>(objectResult.Value);
            Xunit.Assert.NotNull(journeyDto);
            Xunit.Assert.Equal(expectedJourneyDto.Id, journeyDto.Id);
            Xunit.Assert.Equal(expectedJourneyDto.Departure, journeyDto.Departure);
            Xunit.Assert.Equal(expectedJourneyDto.Return, journeyDto.Return);
            Xunit.Assert.Equal(expectedJourneyDto.DepartureStationId, journeyDto.DepartureStationId);
            Xunit.Assert.Equal(expectedJourneyDto.ReturnStationId, journeyDto.ReturnStationId);
            Xunit.Assert.Equal(expectedJourneyDto.CoveredDistanceInMeters, journeyDto.CoveredDistanceInMeters);
            Xunit.Assert.Equal(expectedJourneyDto.DurationInSeconds, journeyDto.DurationInSeconds);
            Xunit.Assert.Equal(expectedJourneyDto.UserId, journeyDto.UserId);
        }
    }

}

