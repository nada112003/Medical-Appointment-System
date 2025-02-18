using System.ComponentModel.DataAnnotations;

namespace BackendAPI.Models
{
    public class Patient : Person
    {
        [Required]
        public string MedicalHistory { get; set; } = string.Empty;

        [Required]
        public string Allergies { get; set; } = string.Empty;

        [Required]
        public string CurrentMedications { get; set; } = string.Empty;

       
        public Profile Profile { get; set; } = null!;
    }
}
