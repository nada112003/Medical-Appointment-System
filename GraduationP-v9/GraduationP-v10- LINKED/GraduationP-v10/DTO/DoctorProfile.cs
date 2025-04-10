using System.Collections.Generic;

namespace GradutionP.DTO
{
    public class DoctorProfile
    {
        public string Photo { get; set; } // Image of the doctor
        public string FullName { get; set; } = string.Empty; // Full name of the doctor
        public string Specialization { get; set; } = string.Empty; // Specialization of the doctor
        public double? Rating { get; set; } // Average rating of the doctor
        public int? ReviewsCount { get; set; } // Number of reviews
        public int ExperienceYears { get; set; } // Years of experience
        public string WorkingHours { get; set; } = string.Empty; // Doctor's working hours
        public string Focus { get; set; } = string.Empty; // Area of focus
        public string Profile { get; set; } = string.Empty; // Detailed profile description
        public string CareerPath { get; set; } = string.Empty; // Career progression
        public string Highlights { get; set; } = string.Empty; // Achievements of the doctor

        // Lists for cities and areas available for the clinic
  
        public string ClinicName { get; set; } // Clinic's name
    }
}
