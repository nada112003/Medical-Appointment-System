using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace GradutionP.Models
{
    public class Doctor : Person
    {
        public string Photo { get; set; }
        public string FullName { get; set; } = string.Empty;  // Full name of the doctor
        public string Specialization { get; set; } = string.Empty;  // Medical specialization
        public double? Rating { get; set; }  // Rating of the doctor
        public int? ReviewsCount { get; set; }  // Number of reviews
        public int ExperienceYears { get; set; }  // Years of experience
        public string WorkingHours { get; set; } = string.Empty;  // Working hours of the doctor
        public string Focus { get; set; } = string.Empty;  // Doctor's area of focus
        public string Profile { get; set; } = string.Empty;  // Detailed profile of the doctor
        public string CareerPath { get; set; } = string.Empty;  // Doctor's career path
        public string Highlights { get; set; } = string.Empty;  // Achievements of the doctor

        [Required]

        public string ClinicName { get; set; }  // Clinic's name

        // Properties to store cities and areas as JSON strings
//        public string CitiesJson { get; set; } = "[]";  // JSON string for cities
//        public string AreasJson { get; set; } = "[]";  // JSON string for areas

//        // Non-mapped properties for cities and areas (they will be serialized/deserialized automatically)
//        [NotMapped]
//        public List<string> Cities
//        {
//            get => JsonSerializer.Deserialize<List<string>>(CitiesJson) ?? new List<string>();
//            set => CitiesJson = JsonSerializer.Serialize(value);
//        }

//        [NotMapped]
//        public List<string> Areas
//        {
//            get => JsonSerializer.Deserialize<List<string>>(AreasJson) ?? new List<string>();
//            set => AreasJson = JsonSerializer.Serialize(value);
//        }

//        // Favorite collection (relationship)
       public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
 }
}
