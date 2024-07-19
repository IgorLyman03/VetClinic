using Appointment.Clients.Interfaces;
using Appointment.Configurations;
using Appointment.DTOs;
using Common.Results;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Appointment.Clients
{
    public class VetAidClient: IVetAidClient
    {
        public VetAidClient(HttpClient httpClient, IOptions<AppointmentConfig> settings)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _vetAidApiUrl = settings.Value.VetAidApiUrl;
        }

        private readonly HttpClient _httpClient;
        private readonly string _vetAidApiUrl;

        public async Task<ServiceResult<VetAidDto>> GetVetAidAsync(int id)
        {
            try
            {
                var url = $"{_vetAidApiUrl}/api/VetAid/{id}";
                var response = await _httpClient.GetAsync($"{url}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var timetables = JsonSerializer.Deserialize<VetAidDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return ServiceResult<VetAidDto>.Success(timetables);
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return ServiceResult<VetAidDto>.Failure(errorContent, response.StatusCode.ToServiceErrorType());
                }
            }
            catch (Exception ex)
            {
                return ServiceResult<VetAidDto>.Failure($"An error occurred while fetching vet aid: {ex.Message}", ServiceErrorType.InternalError);
            }
        }
    }
}
