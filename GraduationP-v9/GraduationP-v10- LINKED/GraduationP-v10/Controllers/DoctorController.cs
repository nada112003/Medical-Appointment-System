using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using GradutionP.Interface;
using GradutionP.DTO;
using System;
using GradutionP.Models;
using GradutionP.Services;
using GraduationP_v10.DTO;

namespace GradutionP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }
        [HttpPut("accept/{requestId}")]
        public ActionResult<AppointmentToReturnDto> AcceptAppointmentRequest(int requestId)
        {
            var result = _doctorService.AcceptAppointmentRequest(requestId);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

        [HttpPost("SetAvailability")]
        public async Task<ActionResult<AvailabilityToReturn>> SetAvailability(int doctorId, AvailabilityToReturn availabilityDto)
        {
            try
            {
                var availability = await _doctorService.SetAvailabilityAsync(doctorId, availabilityDto);
                return CreatedAtAction(nameof(GetAvailability), new { id = availability.DoctorId }, availability);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateAvailability/{availabilityId}")]
        public async Task<ActionResult<AvailabilityToReturn>> UpdateAvailabilityById(int availabilityId, AvailabilityToReturn availabilityDto)
        {
            try
            {
                var availability = await _doctorService.UpdateAvailabilityByIdAsync(availabilityId, availabilityDto);
                return Ok(availability);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpGet("GetAvailability/{id}")]
        public async Task<ActionResult<Availability>> GetAvailability(int id)
        {
            try
            {
                var availability = await _doctorService.GetAvailabilityAsync(id);
                return Ok(availability);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpGet("ViewAppointments/{doctorId}")]
        public IActionResult ViewAppointments(int doctorId)
        {
            var appointments = _doctorService.ViewAppointments(doctorId);
            return Ok(appointments);
        }
    }
}
    

