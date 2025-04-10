using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GradutionP.Models
{
    public class Availability
    {

        [Key]
        public int AvailabilityId { get; set; }

        public double StartTimeInHours { get; set; }
        public double EndTimeInHours { get; set; }
        public string Day { get; set; } = string.Empty;

        // Foreign key for Doctor
        public int DoctorId { get; set; }
        public string Status { get; set; } = "Available";//"Available" , " Booked "
        // ✅ إضافة العلاقة بين Availability و Doctor
        [ForeignKey("DoctorId")]
        public Doctor Doctor { get; set; } = null!;
    }
}
