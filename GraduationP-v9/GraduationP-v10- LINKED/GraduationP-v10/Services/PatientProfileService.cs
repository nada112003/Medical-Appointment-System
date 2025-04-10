using GradutionP.Data;
using GradutionP.Interface;
using GradutionP.Models;
using Microsoft.EntityFrameworkCore;

namespace GradutionP.Services
{
    public class PatientProfileService : IPatientProfileService
    {
        private readonly AppDbContext _context;

        public PatientProfileService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PatientProfile>> GetAllProfiles()
        {
            return await _context.PatientProfiles.ToListAsync();
        }

        public async Task<PatientProfile> GetProfileById(int id)
        {
            return await _context.PatientProfiles.FindAsync(id);
        }

        public async Task<PatientProfile> AddProfile(PatientProfile profile)
        {
            var patientExists = await _context.Patients.AnyAsync(p => p.Id == profile.PatientId);
            if (!patientExists)
                throw new InvalidOperationException("Patient does not exist.");

            var existingProfile = await _context.PatientProfiles
                .FirstOrDefaultAsync(p => p.PatientId == profile.PatientId);
            if (existingProfile != null)
                throw new InvalidOperationException("A profile already exists for this patient.");

            _context.PatientProfiles.Add(profile);
            await _context.SaveChangesAsync();
            return profile;
        }

        public async Task<PatientProfile> UpdateProfile(int id, PatientProfile profile)
        {
            var existingProfile = await _context.PatientProfiles.FindAsync(id);
            if (existingProfile == null) return null;

            if (existingProfile.PatientId != profile.PatientId)
            {
                var patientExists = await _context.Patients.AnyAsync(p => p.Id == profile.PatientId); if (!patientExists)
                    throw new InvalidOperationException("New Patient ID does not exist.");
            }

            existingProfile.FullName = profile.FullName;
            existingProfile.Age = profile.Age;
            existingProfile.Gender = profile.Gender;
            existingProfile.DateOfBirth = profile.DateOfBirth;
            existingProfile.NationalID = profile.NationalID;
            existingProfile.BloodType = profile.BloodType;
            existingProfile.Allergies = profile.Allergies;
            existingProfile.ChronicDiseases = profile.ChronicDiseases;
            existingProfile.CurrentMedications = profile.CurrentMedications;
            existingProfile.ProfilePicture = profile.ProfilePicture;
            existingProfile.InsuranceProvider = profile.InsuranceProvider;
            existingProfile.PatientId = profile.PatientId;

            await _context.SaveChangesAsync();
            return existingProfile;
        }

        public async Task<bool> DeleteProfile(int id)
        {
            var profile = await _context.PatientProfiles.FindAsync(id);
            if (profile == null) return false;

            _context.PatientProfiles.Remove(profile);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}