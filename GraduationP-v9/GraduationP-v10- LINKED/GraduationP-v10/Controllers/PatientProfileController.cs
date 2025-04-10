using GradutionP.Interface;
using GradutionP.Models;
using Microsoft.AspNetCore.Mvc;

namespace GradutionP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientProfileController : ControllerBase
    {
        private readonly IPatientProfileService _profileService;

        public PatientProfileController(IPatientProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var profiles = await _profileService.GetAllProfiles();
            return Ok(profiles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var profile = await _profileService.GetProfileById(id);
            if (profile == null)
            {
                return Ok(new { message = "Patient profile not found" });
            }
            return Ok(profile);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PatientProfile profile)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var createdProfile = await _profileService.AddProfile(profile);
                return CreatedAtAction(nameof(GetById), new { id = createdProfile.ProfileId }, createdProfile);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PatientProfile profile)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var updatedProfile = await _profileService.UpdateProfile(id, profile);
                if (updatedProfile == null)
                    return Ok(new { message = "Patient profile not found, cannot be updated" });
                return Ok(updatedProfile);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _profileService.DeleteProfile(id);
            if (!result)
                return Ok(new { message = "Patient profile not found, cannot be deleted" });
            return Ok(new { message = "Patient profile successfully deleted" });
        }
    }
}