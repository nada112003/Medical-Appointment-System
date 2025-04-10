using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GradutionP.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }

        // Foreign Key for Doctor
        [Required]
        public int DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public Doctor Doctor { get; set; } = null!; // ✅ ربط العلاقة الصحيحة

        // Foreign Key for Patient (يمكن أن يكون المريض NULL إذا لم يتم الحجز بعد)
        public int? PatientId { get; set; }
        [ForeignKey("PatientId")]
        public Patient? Patient { get; set; } // ✅ السماح بالقيمة null لحجز الموعد لاحقًا

        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        [Range(0, 23)]
        public double StartTimeInHours { get; set; }

        [Required]
        [Range(0, 23)]
        public double EndTimeInHours { get; set; }

        [Required]
        [StringLength(10)]
        public string Status { get; set; } = "Upcoming";  // ✅ "Upcoming", "Completed", "Canceled"
        [Timestamp] // 👈 هذا الحقل سيُستخدم لمراقبة التغييرات
        public byte[] RowVersion { get; set; }
    }
}
