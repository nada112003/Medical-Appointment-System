using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace GradutionP.Models
{
    public class AppointmentRequest
    {

        [Key]
        public int RequestId { get; set; }
      
        public DateTime RequestedTime { get; set; }
        public string? RequestsStatus { get; set; } // "Pending", "Accepted", "Rejected"

        // Foreign Key for Doctor
        [Required]
        public int DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public Doctor Doctor { get; set; } = null!; // ✅ ربط العلاقة الصحيحة

    }
}
