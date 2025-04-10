using GradutionP.DTO;
using GradutionP.Interface;
using GradutionP.Models;
using GradutionP.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace GradutionP.Services
{

        public class FeedbackService : IFeedbackService
        {
            private readonly AppDbContext _context;

            public FeedbackService(AppDbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<FeedbackDTO>> GetAllFeedbacksAsync()
            {
                return await _context.Feedbacks
                                     .Select(f => new FeedbackDTO
                                     {
                                         Id = f.Id,
                                         DoctorId = f.DoctorId,
                                         PatientId = f.PatientId,
                                         Comment = f.Comment,
                                         Rating = f.Rating
                                     }).ToListAsync();
            }

            public async Task<IEnumerable<FeedbackDTO>> GetFeedbackByDoctorIdAsync(int doctorId)
            {
                return await _context.Feedbacks
                                     .Where(f => f.DoctorId == doctorId)
                                     .Select(f => new FeedbackDTO
                                     {
                                         Id = f.Id,
                                         DoctorId = f.DoctorId,
                                         PatientId = f.PatientId,
                                         Comment = f.Comment,
                                         Rating = f.Rating
                                     }).ToListAsync();
            }

            public async Task AddFeedbackAsync(FeedbackDTO feedbackDto)
            {
                var feedback = new Feedback
                {
                    DoctorId = feedbackDto.DoctorId,
                    PatientId = feedbackDto.PatientId,
                    Comment = feedbackDto.Comment ?? "",
                    Rating = feedbackDto.Rating
                };

                _context.Feedbacks.Add(feedback);
                await _context.SaveChangesAsync();
            }

            public async Task<bool> DeleteFeedbackAsync(int doctorId, int patientId)
            {
                var feedback = await _context.Feedbacks
                                             .FirstOrDefaultAsync(f => f.DoctorId == doctorId && f.PatientId == patientId);

                if (feedback == null)
                    return false;

                _context.Feedbacks.Remove(feedback);
                await _context.SaveChangesAsync();
                return true;
            }
        }
    }
