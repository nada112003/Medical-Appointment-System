using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace GradutionP.Models
{
    public class Patient : Person
    {
        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
        public PatientProfile Profile { get; set; } = null!;

        // ✅ علاقة المريض بالمواعيد اللي حجزها
    }
}
