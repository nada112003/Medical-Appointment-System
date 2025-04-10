using GradutionP.Service;
using GradutionP.DTO;
using GradutionP.Interface;
using GradutionP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GradutionP.Services;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;

namespace GradutionP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }


        // View all doctor's avaliabilities/appointments with their status (reserved or available)
        #region GetDoctorAppointments
        [HttpGet("doctor/{doctorId}/appointments")]
        public async Task<IActionResult> GetDoctorAppointments(int doctorId)
        {
            var doctorAppointments = await _patientService.GetDoctorAppointmentsAsync(doctorId);
            return Ok(doctorAppointments);
        }
        #endregion



        // Book a specific appointment from the available appointments
        #region BookAppointment
        [HttpPost("book")]
        public async Task<IActionResult> BookAppointment([FromBody] AppointmentDto appointmentDto)
        {
            try
            {
                var appointment = await _patientService.BookAppointmentAsync(
                    appointmentDto.DoctorId,
                    appointmentDto.PatientId,
                    appointmentDto.AvailabilityId);

                return Ok(new
                {
                    message = "Appointment booked successfully.",
                    appointment = new
                    {
                        appointment.AppointmentId,
                        appointment.DoctorId,
                        Doctor_UserName = appointment.Doctor?.Username,
                        appointment.PatientId,
                        Patient_UserName = appointment.Patient?.Username,
                        appointment.AppointmentDate,
                        appointment.StartTimeInHours,
                        appointment.EndTimeInHours,
                        appointment.Status
                    }
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict(new { message = "This appointment slot has already been booked by another patient." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        #endregion


        // ✅ Get Doctor Profile by ID (Async)
        #region GetDoctorProfile
        [HttpGet("{doctorId}")]
        public async Task<ActionResult<DoctorProfile>> GetDoctorProfileAsync(int doctorId)
        {
            if (doctorId <= 0)
            {
                return BadRequest(new { message = "Invalid doctor ID." });
            }

            try
            {
                var doctorProfile = await _patientService.GetDoctorProfileAsync(doctorId);

                if (doctorProfile == null)
                {
                    return NotFound(new { message = $"Doctor with ID {doctorId} not found." });
                }

                return Ok(doctorProfile);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", error = ex.Message });
            }
        }

        #endregion
       

       // Fetch all appointments booked by the patient(with the ability to filter by case)    
        #region GetAppointmentsByStatus
        [HttpGet("patient/{patientId}/appointments")]
        public async Task<IActionResult> GetAppointmentsByStatus(int patientId, [FromQuery] string? status = null)
        {
            var appointments = await _patientService.GetAppointmentsByStatusAsync(patientId, status);
            return Ok(appointments);
        }
        #endregion


        // CancelAppointment
        #region CancelAppointment
        [HttpPut("cancel/{appointmentId}")]
        public async Task<IActionResult> CancelAppointment(int appointmentId)
        {
            try
            {
                if (!await _patientService.CancelAppointmentAsync(appointmentId))
                    return NotFound(new { message = "Appointment not found or already canceled." });

                return Ok(new { message = "Appointment canceled successfully." });
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict(new { message = "Appointment was modified by another process." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }


        #endregion






    }
}

