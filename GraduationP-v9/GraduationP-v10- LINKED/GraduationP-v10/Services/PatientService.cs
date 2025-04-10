using GradutionP.DTO;
using GradutionP.Interface;
using GradutionP.Models;
using GradutionP.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;  
using System.Buffers;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Reflection.Metadata;

namespace GradutionP.Services
{
    public class PatientService : IPatientService
    {
        private readonly AppDbContext _context;

        public PatientService(AppDbContext context)
        {
            _context = context;
        }

        // ✅ 2️⃣ Get all available appointments for a specific doctor
        #region GetDoctorAppointmentsAsync
        public async Task<List<object>> GetDoctorAppointmentsAsync(int doctorId)
        {
            var doctorAppointments = await _context.Availabilities
                .Where(a => a.DoctorId == doctorId)
                .Select(a => new
                {
                    a.AvailabilityId,
                    a.Day,
                    a.StartTimeInHours,
                    a.EndTimeInHours,

                    // 🔹 الحالة تظهر بناءً على وجود موعد بنفس الوقت والتاريخ وحالته Upcoming
                    Status = _context.Appointments.Any(ap =>
                        ap.DoctorId == doctorId &&
                        ap.StartTimeInHours == a.StartTimeInHours &&
                        ap.EndTimeInHours == a.EndTimeInHours &&
                        ap.Status == "Upcoming"
                    ) ? "Booked" : "Available"
                })
                .ToListAsync();

            return doctorAppointments.Cast<object>().ToList();
        }

        #endregion


        // ✅ 2️⃣ Book an appointment by patient
        #region BookAppointmentByPatient


        public async Task<Appointment> BookAppointmentAsync(int doctorId, int patientId, int availabilityId)
        {
            var availability = await _context.Availabilities.FindAsync(availabilityId);
            if (availability == null || availability.DoctorId != doctorId)
                throw new Exception("Invalid availability selection.");

            var existingAppointment = await _context.Appointments
                .Where(ap => ap.DoctorId == doctorId &&
                             ap.StartTimeInHours == availability.StartTimeInHours &&
                             ap.EndTimeInHours == availability.EndTimeInHours &&
                             ap.Status != "Canceled")
                .FirstOrDefaultAsync();

            if (existingAppointment != null)
                throw new Exception("This appointment slot is already booked.");

            var appointment = new Appointment
            {
                DoctorId = doctorId,
                PatientId = patientId,
                AppointmentDate = DateTime.UtcNow,
                StartTimeInHours = availability.StartTimeInHours,
                EndTimeInHours = availability.EndTimeInHours,
                Status = "Upcoming"
            };

            _context.Appointments.Add(appointment);
            availability.Status = "Booked";

            await _context.SaveChangesAsync();

            // ✅ رجع الحجز مع بيانات الدكتور والمريض
            return await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(a => a.AppointmentId == appointment.AppointmentId)
                ?? appointment;
        }

        #endregion



        // ✅ 3️⃣ Get all appointments for a specific patient by status (optional)
        #region GetAppointmentsByStatusAsync
        public async Task<object> GetAppointmentsByStatusAsync(int patientId, string? status = null)
        {
            var query = _context.Appointments
                .Where(a => a.PatientId == patientId);

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(a => a.Status == status);
            }

            var appointments = await query
                .OrderBy(a => a.AppointmentDate)
                .ThenBy(a => a.StartTimeInHours)
                .Select(a => new
                {
                    a.AppointmentId,
                    a.DoctorId,
                    Doctor = a.Doctor != null ? a.Doctor.Username : "No Doctor Assigned",
                    a.PatientId,
                    Patient= a.Patient != null ? a.Patient.Username : "No Patient Assigned",
                    a.AppointmentDate,
                    a.StartTimeInHours,
                    a.EndTimeInHours,
                    a.Status
                })
                .ToListAsync();

            if (!appointments.Any())
            {
                return $"No appointments found for: {status}";
            }

            return appointments;
        }
       

        #endregion





        // ✅ 4️⃣ Cancel an appointment by patient
        #region CancelAppointmentByPatient
        public async Task<bool> CancelAppointmentAsync(int appointmentId)
        {
            // ابحث عن الموعد
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            if (appointment == null || appointment.Status == "Canceled")
                return false;

            if (appointment.Status == "Completed")
                throw new InvalidOperationException("Cannot cancel a completed appointment.");

            // تحديث حالة الموعد
            appointment.Status = "Canceled";

            // تحديث حالة التوفر المرتبط بنفس اليوم والوقت
            var availability = await _context.Availabilities.FirstOrDefaultAsync(a =>
                a.DoctorId == appointment.DoctorId &&
                a.Day == appointment.AppointmentDate.DayOfWeek.ToString() &&
                a.StartTimeInHours == appointment.StartTimeInHours &&
                a.EndTimeInHours == appointment.EndTimeInHours);

            if (availability != null)
            {
                availability.Status = "Available";
            }

            // حفظ التعديلات
            await _context.SaveChangesAsync();
            return true;
        }



        #endregion




        // ✅ 1️⃣ Get doctor profile with details
        #region GetDoctorProfilebyDoctorId
        public async Task<DoctorProfile> GetDoctorProfileAsync(int doctorId)
        {
            var doctor = await _context.Doctors
                .Where(d => d.Id == doctorId)
                .Select(d => new DoctorProfile
                {
                    Photo = d.Photo,
                    FullName = d.FullName, // Corrected from Username to FullName
                    Specialization = d.Specialization,
                    Rating = d.Rating,
                    ReviewsCount = d.ReviewsCount,
                    ExperienceYears = d.ExperienceYears,
                    WorkingHours = d.WorkingHours,
                    Focus = d.Focus,
                    Profile = d.Profile,
                    CareerPath = d.CareerPath,
                    Highlights = d.Highlights,
                    ClinicName = d.ClinicName,
                 
                })
                .FirstOrDefaultAsync();

            if (doctor == null)
            {
                throw new KeyNotFoundException($"Doctor with ID {doctorId} not found.");
            }

            return doctor;
        } 
        #endregion

       
    }
}
