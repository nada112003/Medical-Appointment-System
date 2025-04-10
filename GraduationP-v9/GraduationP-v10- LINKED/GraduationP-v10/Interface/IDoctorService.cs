using DoctorPart.Dtos;
using GraduationP_v10.DTO;
using GradutionP.DTO;
using GradutionP.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GradutionP.Interface
{
    public interface IDoctorService
    {
        public IEnumerable<DoctorToReturnDto> ViewAppointments(int doctorId);

        public Task<AvailabilityToReturn> UpdateAvailabilityByIdAsync(int availabilityId, AvailabilityToReturn availabilityDto);
        Task<AvailabilityToReturn> SetAvailabilityAsync(int doctorId, AvailabilityToReturn availabilityDto);
        public AppointmentToReturnDto AcceptAppointmentRequest(int requestId);
        public  Task<Availability> GetAvailabilityAsync(int id);

    }
}
