using GradutionP.DTO;
using GradutionP.Models;

namespace GradutionP.Interface
{
    public interface IFavoriteService
    {
        Task<string> AddFavorite(FavoriteDto favoriteDto);
        Task<List<DoctorDto>> GetFavorites(int patientId);
        Task<string> RemoveFavorite(FavoriteDto favoriteDto);


    }

}


