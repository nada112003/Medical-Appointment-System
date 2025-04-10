using GradutionP.Data;
using GradutionP.Interface;
using GradutionP.Models;
using Microsoft.AspNetCore.Mvc;

namespace GradutionP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FeedbackController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetAllFeedback()
        {
            var feedbacks = _context.Feedbacks.ToList();

            return Ok(feedbacks.Count > 0 ? feedbacks : new { message = "No feedback available." });
        }

        [HttpPost]
        public async Task<IActionResult> AddFeedback([FromBody] Feedback feedback)
        {
            if (feedback == null || string.IsNullOrWhiteSpace(feedback.Comment) || feedback.Rating < 1 || feedback.Rating > 5)
            {
                return Ok(new { message = "Invalid feedback data." });
            }

            var doctorExists = _context.Doctors.Any(d => d.Id == feedback.DoctorId);
            if (!doctorExists)
            {
                return Ok(new { message = "Doctor not found." });
            }

            var patientExists = _context.Patients.Any(p => p.Id == feedback.PatientId);
            if (!patientExists)
            {
                return Ok(new { message = "Patient not found." });
            }

            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Feedback added successfully." });
        }

        [HttpGet("doctor/{doctorId}")]
        public IActionResult GetDoctorFeedback(int doctorId)
        {
            var doctorExists = _context.Doctors.Any(d => d.Id == doctorId);
            if (!doctorExists)
            {
                return Ok(new { message = "Doctor not found." });
            }

            var feedbacks = _context.Feedbacks.Where(f => f.DoctorId == doctorId).ToList();

            return Ok(feedbacks.Count > 0 ? feedbacks : new { message = "No feedback found for this doctor." });
        }

        [HttpDelete("doctor/{doctorId}")]
        public async Task<IActionResult> DeleteDoctorFeedback(int doctorId)
        {
            var feedbacks = _context.Feedbacks.Where(f => f.DoctorId == doctorId).ToList();

            if (feedbacks.Count == 0)
            {
                return Ok(new { message = "Doctor not found or no feedback available for deletion." });
            }

            _context.Feedbacks.RemoveRange(feedbacks);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Feedback deleted successfully." });
        }
    }
}

