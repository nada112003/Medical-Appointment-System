using GradutionP.Data;
using GradutionP.Interface;
using GradutionP.Models;
using GradutionP.DTO;
using Microsoft.EntityFrameworkCore;
using DoctorPart.Dtos;
using GraduationP_v10.DTO;


namespace GradutionP.Service
{
    public class DoctorService : IDoctorService
    {
        private readonly AppDbContext _context;

        public DoctorService(AppDbContext context)
        {
            _context = context;
        }
        public AppointmentToReturnDto AcceptAppointmentRequest(int requestId)
        {
            var request = _context.AppointmentRequests.Find(requestId);
            if (request != null)
            {
                var doctor = _context.Doctors.Find(request.DoctorId);
                if (doctor != null)
                {
                    request.RequestsStatus = "Accepted";
                    var appointment = _context.Appointments.FirstOrDefault(a => a.DoctorId == request.DoctorId && a.AppointmentDate == request.RequestedTime);
                    if (appointment != null)
                    {
                        appointment.Status = "Accepted";
                    }
                    _context.SaveChanges();
                }
                else
                {
                    request.RequestsStatus = "Rejected";
                    var appointment = _context.Appointments.FirstOrDefault(a => a.DoctorId == request.DoctorId && a.AppointmentDate == request.RequestedTime);
                    if (appointment != null)
                    {
                        appointment.Status = "Rejected";
                    }
                    _context.SaveChanges();
                }

                return new AppointmentToReturnDto
                {
                    Username = request.Doctor.Username,
                    RequestId = request.RequestId,
                    RequestsStatus = request.RequestsStatus,
                    RequestedTime = request.RequestedTime
                };
            }

            return null;
        }


        public async Task<AvailabilityToReturn> SetAvailabilityAsync(int doctorId, AvailabilityToReturn availabilityDto)
        {
            var doctor = await _context.Doctors.FindAsync(doctorId);
            if (doctor == null)
            {
                throw new Exception("Doctor not found");
            }
            if (availabilityDto.StartTimeInHours >= availabilityDto.EndTimeInHours)
            {
                throw new Exception("Start time must be less than end time.");
            }

            // Check if the availability already exists
            var existingAvailability = await _context.Availabilities
                .FirstOrDefaultAsync(a => a.DoctorId == doctorId &&
                                          a.Day == availabilityDto.Day &&
                                          a.StartTimeInHours == availabilityDto.StartTimeInHours &&
                                          a.EndTimeInHours == availabilityDto.EndTimeInHours);
                                         
            if (existingAvailability != null)
            {
                throw new Exception("This availability already exists.");
            }

            var availability = new Availability
            {
                StartTimeInHours = availabilityDto.StartTimeInHours,
                EndTimeInHours = availabilityDto.EndTimeInHours,
                Day = availabilityDto.Day,
                DoctorId = doctorId
            };

            _context.Availabilities.Add(availability);
            await _context.SaveChangesAsync();

            return new AvailabilityToReturn
            {
                DoctorId = availability.DoctorId,
                Day = availability.Day,
                StartTimeInHours = availability.StartTimeInHours,
                EndTimeInHours = availability.EndTimeInHours,
                // Username = doctor.Username // Fetch and include the doctor's username
            };
        }

        public async Task<AvailabilityToReturn> UpdateAvailabilityByIdAsync(int availabilityId, AvailabilityToReturn availabilityDto)
        {
            var availability = await _context.Availabilities.FindAsync(availabilityId);
            if (availability == null)
            {
                throw new Exception("Availability not found");
            }

            var doctor = await _context.Doctors.FindAsync(availability.DoctorId);
            if (doctor == null)
            {
                throw new Exception("Doctor not found");
            }

            if (availabilityDto.StartTimeInHours >= availabilityDto.EndTimeInHours)
            {
                throw new Exception("Start time must be less than end time.");
            }

            availability.StartTimeInHours = availabilityDto.StartTimeInHours;
            availability.EndTimeInHours = availabilityDto.EndTimeInHours;
            availability.Day = availabilityDto.Day;

            await _context.SaveChangesAsync();

            return new AvailabilityToReturn
            {
                DoctorId = availability.DoctorId,
                Day = availability.Day,
                StartTimeInHours = availability.StartTimeInHours,
                EndTimeInHours = availability.EndTimeInHours,
                //Username = doctor.Username
            };
        }



        public async Task<Availability> GetAvailabilityAsync(int id)
        {
            var availability = await _context.Availabilities.FindAsync(id);
            if (availability == null)
            {
                throw new Exception("Availability not found");
            }

            return availability;
        }
        public IEnumerable<DoctorToReturnDto> ViewAppointments(int doctorId)
        {
            return _context.Appointments
                .Where(a => a.DoctorId == doctorId)
                .Include(a => a.Doctor)
                .Select(a => new DoctorToReturnDto
                {
                    AppointmentId = a.AppointmentId,
                    AppointmentDate = a.AppointmentDate,
                    UserName = a.Doctor.Username,
                    Specialization = a.Doctor.Specialization,
                    //IsAvailable = a.Doctor.IsAvailable
                })
                .ToList();
        }
    }
}









    
