using GradutionP.DTO;
namespace GradutionP.Interface
{
    public interface IFeedbackService
    {
        Task<IEnumerable<FeedbackDTO>> GetAllFeedbacksAsync();
        Task<IEnumerable<FeedbackDTO>> GetFeedbackByDoctorIdAsync(int doctorId);
        Task AddFeedbackAsync(FeedbackDTO feedback);
        Task<bool> DeleteFeedbackAsync(int doctorId, int patientId);
    }
}
