using System;
using GradutionP.Models;
using GradutionP.Data;
using GradutionP.DTO;
using Microsoft.EntityFrameworkCore;
using GradutionP.Interface;

namespace GradutionP.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly AppDbContext _context;

        public FavoriteService(AppDbContext context)
        {
            _context = context;
        }

        // ✅Add a doctor to favorites
        #region AddDoctorToFavorite
        public async Task<string> AddFavorite(FavoriteDto favoriteDto)
        {
            try
            {
                var existingFavorite = await _context.Favorites
                    .FirstOrDefaultAsync(f => f.PatientId == favoriteDto.PatientId && f.DoctorId == favoriteDto.DoctorId);

                if (existingFavorite != null)
                {
                    return "Doctor is already in favorites.";
                }

                var favorite = new Favorite { PatientId = favoriteDto.PatientId, DoctorId = favoriteDto.DoctorId };
                await _context.Favorites.AddAsync(favorite);
                await _context.SaveChangesAsync();
                return "Doctor added to favorites successfully.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddFavorite: {ex.Message}");
                return "An error occurred while adding the doctor to favorites.";
            }
        }
        #endregion


        // ✅ Get all favorite doctors by patientId
        #region GetDoctorsFavoritesbyPatientId
        public async Task<List<DoctorDto>> GetFavorites(int patientId)
        {
            try
            {
                return await _context.Favorites
                    .Where(f => f.PatientId == patientId)
                    .Select(f => new DoctorDto
                    {
                        Photo = f.Doctor.Photo,
                        FullName = f.Doctor.FullName,
                        Specialization = f.Doctor.Specialization
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetFavorites: {ex.Message}");
                return new List<DoctorDto>(); // بيرجع قائمة فاضية في حالة الخطأ
            }
        }
        #endregion



        // ✅ Delete a doctor from favorites
        #region RemoveFavorite
        public async Task<string> RemoveFavorite(FavoriteDto favoriteDto)
        {
            try
            {
                var favorite = await _context.Favorites
                    .FirstOrDefaultAsync(f => f.PatientId == favoriteDto.PatientId && f.DoctorId == favoriteDto.DoctorId);

                if (favorite == null)
                {
                    return "Doctor is not in favorites.";
                }

                _context.Favorites.Remove(favorite);
                await _context.SaveChangesAsync();
                return "Doctor removed from favorites successfully.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in RemoveFavorite: {ex.Message}");
                return "An error occurred while removing the doctor from favorites.";
            }
        } 
        #endregion
    }
}
