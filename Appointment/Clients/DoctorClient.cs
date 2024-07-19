using Appointment.Configurations;
using Common.Results;
using Appointment.DTOs;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Appointment.Clients.Interfaces;

namespace Appointment.Clients
{
    public class DoctorClient: IDoctorClient
    {

        public DoctorClient(HttpClient httpClient, IOptions<AppointmentConfig> settings)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _doctorApiUrl = settings.Value.DoctorApiUrl;
        }

        private readonly HttpClient _httpClient;
        private readonly string _doctorApiUrl;

        public async Task<ServiceResult<IEnumerable<DoctorTimetableSegmentDto>>> GetTimetableByDatesAsync(string userId, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            try
            {
                var url = $"{_doctorApiUrl}/api/DoctorTimeTable/ByDates";
                var formattedStartDate = Uri.EscapeDataString(startDate.ToString("yyyy-MM-dd'T'HH:mm:ss+00:00"));
                var formattedEndDate = Uri.EscapeDataString(endDate.ToString("yyyy-MM-dd'T'HH:mm:ss+00:00"));
                var response = await _httpClient.GetAsync($"{url}?userId={userId}&startDate={formattedStartDate}&endDate={formattedEndDate}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var timetables = JsonSerializer.Deserialize<IEnumerable<DoctorTimetableSegmentDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return ServiceResult<IEnumerable<DoctorTimetableSegmentDto>>.Success(timetables);
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return ServiceResult<IEnumerable<DoctorTimetableSegmentDto>>.Failure(errorContent, response.StatusCode.ToServiceErrorType() );
                }
            }
            catch (Exception ex)
            {
                return ServiceResult<IEnumerable<DoctorTimetableSegmentDto>>.Failure($"An error occurred while fetching doctor timetables: {ex.Message}", ServiceErrorType.InternalError);
            }
        }
    }
}

