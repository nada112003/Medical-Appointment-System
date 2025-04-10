using Microsoft.AspNetCore.Mvc;
using GradutionP.DTO;
using GradutionP.Interface;
namespace GradutionP.Controllers
{
    [ApiController]
    [Route("api/favorites")]
    public class FavoriteController : ControllerBase
    {
        private readonly IFavoriteService _favoriteService;

        public FavoriteController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        // add a doctor to favorites
        #region AddDoctorToFavorite
        [HttpPost]
        public async Task<IActionResult> AddFavorite([FromBody] FavoriteDto favoriteDto)
        {
            var result = await _favoriteService.AddFavorite(favoriteDto);
            return Ok(new { message = result });
        }


        #endregion


        // get all favorite doctors by patientId
        #region GetFavoritesbypatientId
        [HttpGet("{patientId}")]
        public async Task<IActionResult> GetFavorites(int patientId)
        {
            var doctors = await _favoriteService.GetFavorites(patientId);

            if (doctors == null || !doctors.Any())
            {
                return NotFound(new { message = "You have no favorite doctors." });
            }

            return Ok(doctors);
        }
        #endregion


        // remove a doctor from favorites
        #region RemoveDoctorFromFavorite
        [HttpDelete]
        public async Task<IActionResult> RemoveFavorite([FromBody] FavoriteDto favoriteDto)
        {
            var result = await _favoriteService.RemoveFavorite(favoriteDto);
            return Ok(new { message = result });
        } 
        #endregion
    }

}
