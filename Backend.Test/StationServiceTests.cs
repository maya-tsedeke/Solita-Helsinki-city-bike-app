
namespace Backend.Test;
[TestFixture]
public class StationServiceTests
{
    private Mock<IStationRepository> _stationRepositoryMock;
    private IMapper _mapper;
    private Mock<IJourneyRepository> _journeyRepositoryMock;


    [SetUp]
    public void Setup()
    {
        _stationRepositoryMock = new Mock<IStationRepository>();
        _journeyRepositoryMock = new Mock<IJourneyRepository>();
        _mapper = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<StationDto, Station>();
            cfg.CreateMap<Station, StationDto>();
        }).CreateMapper();
    }

    [Test]
    public async Task CreateStationAsync_StationDtoIsValid_ReturnsStationDto()
    {
        // Arrange
        var stationDto = new StationDto
        {
            FID = 1,
            ID = 1,
            Nimi = "Test Station",
            Namn = "Test Station",
            Name = "Test Station",
            Osoite = "Test Address",
            Address = "Test Address",
            Kaupunki = "Test City",
            Stad = "Test City",
            Operaattor = "Test Operator",
            Kapasiteet = "Test Capacity",
            x = 1.0,
            y = 2.0
        };

        var expectedStation = new Station
        {
            FID = 1,
            ID = 1,
            Nimi = "Test Station",
            Namn = "Test Station",
            Name = "Test Station",
            Osoite = "Test Address",
            Address = "Test Address",
            Kaupunki = "Test City",
            Stad = "Test City",
            Operaattor = "Test Operator",
            Kapasiteet = "Test Capacity",
            x = 1.0,
            y = 2.0
        };
        _stationRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Station>())).Callback<Station>(station => station.ID = expectedStation.ID).Returns(Task.FromResult(0));



        // Create service and controller
        //var stationService = new StationService(_stationRepositoryMock.Object, _mapper, _journeyRepositoryMock.Object);
        var stationService = new StationService(_stationRepositoryMock.Object, _mapper, It.IsAny<IJourneyRepository>());

        var stationController = new StationController(stationService);

        // Act
        var result = await stationService.CreateStationAsync(stationDto);

        // Assert
        Assert.NotNull(result);
        Xunit.Assert.Equal(expectedStation.ID, result.ID);
        Xunit.Assert.Equal(expectedStation.Nimi, result.Nimi);
        Xunit.Assert.Equal(expectedStation.Namn, result.Namn);
        Xunit.Assert.Equal(expectedStation.Name, result.Name);
        Xunit.Assert.Equal(expectedStation.Osoite, result.Osoite);
        Xunit.Assert.Equal(expectedStation.Address, result.Address);
        Xunit.Assert.Equal(expectedStation.Kaupunki, result.Kaupunki);
        Xunit.Assert.Equal(expectedStation.Stad, result.Stad);
        Xunit.Assert.Equal(expectedStation.Operaattor, result.Operaattor);
        Xunit.Assert.Equal(expectedStation.Kapasiteet, result.Kapasiteet);
        Xunit.Assert.Equal(expectedStation.x, result.x);
        Xunit.Assert.Equal(expectedStation.y, result.y);
    }
    [Test]
    public async Task UpdateStation_ValidDto_ReturnsOkResult()
    {
        // Arrange
        int id = 1;
        var stationDto = new StationDto
        {
            FID = 100,
            ID = 100,
            Nimi = "Test Station",
            Namn = "Test Station",
            Name = "Test Station",
            Osoite = "Test Address",
            Address = "Test Address",
            Kaupunki = "Helsinki",
            Stad = "Espoo",
            Operaattor = "Finland Operator",
            Kapasiteet = "10",
            x = 60.12345,
            y = 24.12345
        };
        var validatorMock = new Mock<IValidator<StationDto>>();
        validatorMock.Setup(validator => validator.ValidateAsync(stationDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        var stationServiceMock = new Mock<IStationService>();
        stationServiceMock.Setup(service => service.UpdateStationAsync(id, stationDto))
            .ReturnsAsync(stationDto);
        var stationController = new StationController(stationServiceMock.Object, validatorMock.Object);

        // Act
        var result = await stationController.UpdateStation(id, stationDto);

        // Assert
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var message = Xunit.Assert.IsType<string>(okResult.Value.GetType().GetProperty("message").GetValue(okResult.Value));
        Xunit.Assert.Equal("Station updated successfully.", message);
    }

}
[TestFixture]
public class StationControllerTests
{
    private Mock<IStationService> _stationServiceMock;
    private StationController _stationController;

    [SetUp]
    public void Setup()
    {
        _stationServiceMock = new Mock<IStationService>();
        _stationController = new StationController(_stationServiceMock.Object);
    }

    [Test]
    public async Task CreateStation_StationDtoIsValid_ReturnsCreatedAtActionResultWithStationDto()
    {
        // Arrange
        var stationDto = new StationDto
        {
            FID = 1,
            ID = 1,
            Nimi = "Test Station",
            Namn = "Test Station",
            Name = "Test Station",
            Osoite = "Test Address",
            Address = "Test Address",
            Kaupunki = "Test City",
            Stad = "Test City",
            Operaattor = "Test Operator",
            Kapasiteet = "Test Capacity",
            x = 1.0,
            y = 2.0
        };

        var createdStationDto = new StationDto
        {
            FID = 1,
            ID = 1,
            Nimi = "Test Station",
            Namn = "Test Station",
            Name = "Test Station",
            Osoite = "Test Address",
            Address = "Test Address",
            Kaupunki = "Test City",
            Stad = "Test City",
            Operaattor = "Test Operator",
            Kapasiteet = "Test Capacity",
            x = 1.0,
            y = 2.0
        };

        _stationServiceMock.Setup(service => service.CreateStationAsync(stationDto)).ReturnsAsync(createdStationDto);

        // Act
        var result = await _stationController.CreateStation(stationDto) as CreatedAtActionResult;

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(nameof(_stationController.GetStation), result.ActionName);
        Assert.AreEqual(createdStationDto.ID, result.RouteValues["id"]);
        Assert.AreEqual(createdStationDto, result.Value);
    }

    [Test]
    public async Task CreateStation_StationDtoIsInvalid_ReturnsBadRequestObjectResultWithValidationErrors()
    {
        // Arrange
        var stationDto = new StationDto();

        var validationErrors = new List<ValidationFailure>
    {
        new ValidationFailure("FID", "FID is required."),
        new ValidationFailure("ID", "ID is required."),
        new ValidationFailure("Nimi", "Nimi is required."),
        new ValidationFailure("Namn", "Namn is required."),
        new ValidationFailure("Name", "Name is required."),
        new ValidationFailure("Osoite", "Osoite is required."),
        new ValidationFailure("Address", "Address is required."),
        new ValidationFailure("Kaupunki", "Kaupunki is required."),
        new ValidationFailure("Stad", "Stad is required."),
        new ValidationFailure("Operaattor", "Operaattor is required."),
        new ValidationFailure("Kapasiteet", "Kapasiteet is required."),
        new ValidationFailure("x", "x is required."),
        new ValidationFailure("y", "y is required.")
    };

        var validationResult = new ValidationResult(validationErrors);

        var expectedErrorMessage = string.Join("\n", validationErrors.Select(error => $"{error.PropertyName}: {error.ErrorMessage}"));

        var expectedBadRequestObjectResult = new BadRequestObjectResult(new { message = expectedErrorMessage });

        var validatorMock = new Mock<IValidator<StationDto>>();
        validatorMock.Setup(validator => validator.ValidateAsync(stationDto, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

        var stationServiceMock = new Mock<IStationService>();
        stationServiceMock.Setup(service => service.CreateStationAsync(stationDto)).ThrowsAsync(new Exception(expectedErrorMessage));

        var stationController = new StationController(stationServiceMock.Object, validatorMock.Object);

        // Act
        var result = await stationController.CreateStation(stationDto);

        // Assert
        var badRequestObjectResult = Xunit.Assert.IsType<BadRequestObjectResult>(result);
        Xunit.Assert.Equal(expectedBadRequestObjectResult.StatusCode, badRequestObjectResult.StatusCode);
        Xunit.Assert.Equal(expectedBadRequestObjectResult.Value, badRequestObjectResult.Value, new ObjectComparer());
    }







    public class ObjectComparer : IEqualityComparer<object>
    {
        public new bool Equals(object x, object y)
        {
            var serializedX = JsonConvert.SerializeObject(x);
            var serializedY = JsonConvert.SerializeObject(y);

            return string.Equals(serializedX, serializedY);
        }

        public int GetHashCode(object obj)
        {
            return obj.GetHashCode();
        }
    }


}

