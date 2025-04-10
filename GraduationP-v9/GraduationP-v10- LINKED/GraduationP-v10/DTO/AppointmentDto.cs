
    namespace GradutionP.DTO
    {
        public class AppointmentDto
        {
            public int DoctorId { get; set; }
            public int PatientId { get; set; }
            public int AvailabilityId { get; set; } // ✅ اختيار الموعد من المتاحين
        }
    }




