using GradutionP.Models;

namespace GradutionP.Interface
{
    public interface IPatientProfileService
    {
        Task<IEnumerable<PatientProfile>> GetAllProfiles();
        Task<PatientProfile> GetProfileById(int id);
        Task<PatientProfile> AddProfile(PatientProfile profile);
        Task<PatientProfile> UpdateProfile(int id, PatientProfile profile);
        Task<bool> DeleteProfile(int id);
    }
}
