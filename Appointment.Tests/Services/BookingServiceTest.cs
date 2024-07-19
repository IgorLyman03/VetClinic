using Xunit;
using Moq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Appointment.Services.Interfaces;
using Appointment.Services;
using Appointment.Repositories.Interfaces;
using Appointment.DTOs;
using Appointment.Clients;
using Common.Results;
using Appointment.Data.Entities;
using Appointment.Clients.Interfaces;
using Appointment.Model;

public class BookingServiceTests
{
    private readonly Mock<IDoctorClient> _mockDoctorClient;
    private readonly Mock<IBookingRepository> _mockBookingRepository;
    private readonly Mock<IVetAidClient> _mockVetAidClient;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<BookingService>> _mockLogger;
    private readonly BookingService _bookingService;


    public BookingServiceTests()
    {
        _mockDoctorClient = new Mock<IDoctorClient>();
        _mockBookingRepository = new Mock<IBookingRepository>();
        _mockVetAidClient = new Mock<IVetAidClient>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<BookingService>>();

        _bookingService = new BookingService(
            _mockBookingRepository.Object,
            _mockMapper.Object,
            _mockDoctorClient.Object,
            _mockVetAidClient.Object,
            _mockLogger.Object
            );
    }

    [Fact]
    public async Task AddAsync_ValidBooking_ReturnsSuccess()
    {
        // Arrange
        var bookingDto = new BookingDto
        {
            DoctorId = "1",
            StartDate = DateTimeOffset.Parse("2020-01-01T06:00:00Z"),
            EndDate = DateTimeOffset.Parse("2020-01-01T07:00:00Z"),
            VetAidId = 1
        };

        var timetableSegment = new List<DoctorTimetableSegmentDto>
        {
            new DoctorTimetableSegmentDto { StartTime = DateTimeOffset.Parse("2020-01-01T05:00:00Z"), EndTime = DateTimeOffset.Parse("2020-01-01T09:00:00Z") }
        };

        _mockDoctorClient.Setup(c => c.GetTimetableByDatesAsync(It.IsAny<string>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()))
            .ReturnsAsync(ServiceResult<IEnumerable<DoctorTimetableSegmentDto>>.Success(timetableSegment));


        _mockBookingRepository.Setup(r => r.IsFreeToBooking(It.IsAny<string>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()))
            .ReturnsAsync(ServiceResult<bool>.Success(true));

        _mockVetAidClient.Setup(c => c.GetVetAidAsync(It.IsAny<int>()))
            .ReturnsAsync(ServiceResult<VetAidDto>.Success(new VetAidDto { Duration = TimeSpan.FromHours(1) }));

        var booking = new Booking();
        _mockMapper.Setup(m => m.Map<Booking>(It.IsAny<BookingDto>())).Returns(booking);
        _mockMapper.Setup(m => m.Map<BookingDto>(It.IsAny<Booking>())).Returns(bookingDto);

        _mockBookingRepository.Setup(r => r.AddAsync(It.IsAny<Booking>()))
            .ReturnsAsync(ServiceResult<Booking>.Success(booking));

        // Act
        var result = await _bookingService.AddAsync(bookingDto);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(bookingDto, result.Value);
    }

    [Fact]
    public async Task AddAsync_DoctorNotAvailable_ReturnsFailure()
    {
        // Arrange
        var bookingDto = new BookingDto
        {
            DoctorId = "1",
            StartDate = DateTimeOffset.Parse("2020-01-01T06:00:00Z"),
            EndDate = DateTimeOffset.Parse("2020-01-01T07:00:00Z"),
            VetAidId = 1
        };
        var timetableSegment = new List<DoctorTimetableSegmentDto>
        {
            new DoctorTimetableSegmentDto { StartTime = DateTimeOffset.Parse("2020-01-01T08:00:00Z"), EndTime = DateTimeOffset.Parse("2020-01-01T09:00:00Z") }
        };
        _mockDoctorClient.Setup(c => c.GetTimetableByDatesAsync(It.IsAny<string>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()))
            .ReturnsAsync(ServiceResult<IEnumerable<DoctorTimetableSegmentDto>>.Success(timetableSegment));

        // Act
        var result = await _bookingService.AddAsync(bookingDto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("This doctor is not working at this time", result.Error.Message);
    }

    [Fact]
    public async Task AddAsync_TimeSlotNotFree_ReturnsFailure()
    {
        // Arrange
        var bookingDto = new BookingDto
        {
            DoctorId = "1",
            StartDate = DateTimeOffset.Parse("2020-01-01T06:00:00Z"),
            EndDate = DateTimeOffset.Parse("2020-01-01T07:00:00Z"),
            VetAidId = 1
        };
        var timetableSegment = new List<DoctorTimetableSegmentDto>
        {
            new DoctorTimetableSegmentDto { StartTime = DateTimeOffset.Parse("2020-01-01T05:00:00Z"), EndTime = DateTimeOffset.Parse("2020-01-01T09:00:00Z") }
        };
        _mockDoctorClient.Setup(c => c.GetTimetableByDatesAsync(It.IsAny<string>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()))
            .ReturnsAsync(ServiceResult<IEnumerable<DoctorTimetableSegmentDto>>.Success(timetableSegment));
        _mockBookingRepository.Setup(r => r.IsFreeToBooking(It.IsAny<string>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()))
            .ReturnsAsync(ServiceResult<bool>.Success(false));

        // Act
        var result = await _bookingService.AddAsync(bookingDto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("The time for booking is busy or an error occurred", result.Error.Message);
    }

    [Fact]
    public async Task AddAsync_InvalidVetAidDuration_ReturnsFailure()
    {
        // Arrange
        var bookingDto = new BookingDto
        {
            DoctorId = "1",
            StartDate = DateTimeOffset.Parse("2020-01-01T06:00:00Z"),
            EndDate = DateTimeOffset.Parse("2020-01-01T07:00:00Z"),
            VetAidId = 1
        };
        var timetableSegment = new List<DoctorTimetableSegmentDto>
        {
            new DoctorTimetableSegmentDto { StartTime = DateTimeOffset.Parse("2020-01-01T05:00:00Z"), EndTime = DateTimeOffset.Parse("2020-01-01T09:00:00Z") }
        };
        _mockDoctorClient.Setup(c => c.GetTimetableByDatesAsync(It.IsAny<string>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()))
            .ReturnsAsync(ServiceResult<IEnumerable<DoctorTimetableSegmentDto>>.Success(timetableSegment));
        _mockBookingRepository.Setup(r => r.IsFreeToBooking(It.IsAny<string>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()))
            .ReturnsAsync(ServiceResult<bool>.Success(true));
        _mockVetAidClient.Setup(c => c.GetVetAidAsync(It.IsAny<int>()))
            .ReturnsAsync(ServiceResult<VetAidDto>.Success(new VetAidDto { Duration = TimeSpan.FromHours(2) }));

        // Act
        var result = await _bookingService.AddAsync(bookingDto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("The vet aid does not exist or its duration does not match", result.Error.Message);
    }

    [Fact]
    public async Task AddAsync_ExceptionThrown_ReturnsFailure()
    {
        // Arrange
        var bookingDto = new BookingDto
        {
            DoctorId = "1",
            StartDate = DateTimeOffset.Parse("2020-01-01T06:00:00Z"),
            EndDate = DateTimeOffset.Parse("2020-01-01T07:00:00Z"),
            VetAidId = 1
        };
        _mockDoctorClient.Setup(c => c.GetTimetableByDatesAsync(It.IsAny<string>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()))
            .ThrowsAsync(new Exception("Simulated error"));

        // Act
        var result = await _bookingService.AddAsync(bookingDto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Error adding booking", result.Error.Message);
    }

    [Fact]
    public async Task GetFreeTimeAsync_ValidInput_ReturnsAvailableSlots()
    {
        // Arrange
        string doctorId = "1";
        var startDate = DateTimeOffset.Parse("2020-01-01T04:00:00Z");
        var endDate = DateTimeOffset.Parse("2020-01-02T04:00:00Z");
        int vetAidId = 1;

        var vetAid = new VetAidDto { Duration = TimeSpan.FromMinutes(30) };
        _mockVetAidClient.Setup(c => c.GetVetAidAsync(vetAidId))
            .ReturnsAsync(ServiceResult<VetAidDto>.Success(vetAid));

        var doctorTimetable = new List<DoctorTimetableSegmentDto>
        {
            new DoctorTimetableSegmentDto { StartTime = DateTimeOffset.Parse("2020-01-01T04:00:00Z"), EndTime = DateTimeOffset.Parse("2020-01-01T12:00:00Z")}
        };
        _mockDoctorClient.Setup(c => c.GetTimetableByDatesAsync(doctorId, startDate, endDate))
            .ReturnsAsync(ServiceResult<IEnumerable<DoctorTimetableSegmentDto>>.Success(doctorTimetable));

        var bookings = new List<BookingDto>
        {
            new BookingDto { StartDate = DateTimeOffset.Parse("2020-01-01T05:00:00Z"), EndDate = DateTimeOffset.Parse("2020-01-01T06:00:00Z") },
            new BookingDto { StartDate = DateTimeOffset.Parse("2020-01-01T06:00:00Z"), EndDate = DateTimeOffset.Parse("2020-01-01T07:00:00Z") }
            
        };

        _mockBookingRepository.Setup(r => r.GetFilteredBookingsAsync(It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<int?>(), It.IsAny<DateTimeOffset?>(), It.IsAny<DateTimeOffset?>()))
            .ReturnsAsync(ServiceResult<IEnumerable<Booking>>.Success(new List<Booking>()));

        _mockMapper.Setup(m => m.Map<IEnumerable<BookingDto>>(It.IsAny<IEnumerable<Booking>>())).Returns(bookings);

        // Act
        var result = await _bookingService.GetFreeTimeAsync(doctorId, startDate, endDate, vetAidId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(12, result.Value.Count());
        Assert.Contains(startDate, result.Value);
        Assert.Contains(DateTimeOffset.Parse("2020-01-01T07:00:00Z"), result.Value);
        Assert.DoesNotContain(DateTimeOffset.Parse("2020-01-01T05:00:00Z"), result.Value);
    }

    [Fact]
    public async Task GetFreeTimeAsync_VetAidNotFound_ReturnsFailure()
    {
        // Arrange
        string doctorId = "123";
        DateTimeOffset startDate = DateTimeOffset.Now;
        DateTimeOffset endDate = startDate.AddDays(1);
        int vetAidId = 1;

        _mockVetAidClient.Setup(c => c.GetVetAidAsync(vetAidId))
            .ReturnsAsync(ServiceResult<VetAidDto>.Failure(new ServiceError("VetAid not found", ServiceErrorType.NotFound)));

        // Act
        var result = await _bookingService.GetFreeTimeAsync(doctorId, startDate, endDate, vetAidId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("VetAid not found", result.Error.Message);
    }
}