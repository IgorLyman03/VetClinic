using Appointment.DTOs;
using Common.Results;

namespace Appointment.Clients.Interfaces
{
    public interface IVetAidClient
    {
        Task<ServiceResult<VetAidDto>> GetVetAidAsync(int id);
    }
}
