using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace GradutionP.Models
{
    public class PatientProfile
    {
        [Key]
        public int ProfileId { get; set; }

        public string FullName { get; set; }
        public string? ProfilePicture { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public int Age { get; set; }
        public string? NationalID { get; set; }
        public string? BloodType { get; set; }
        public string? ChronicDiseases { get; set; }
        public string? Allergies { get; set; }
        public string? CurrentMedications { get; set; }
        public string? InsuranceProvider { get; set; }

        [Required]
        public int PatientId { get; set; }

        [ForeignKey("PatientId")]
        [ValidateNever] 
        public Patient Patient { get; set; } = null!;
    }
}