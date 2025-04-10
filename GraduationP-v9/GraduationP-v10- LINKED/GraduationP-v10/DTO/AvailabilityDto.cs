namespace GradutionP.DTO
{
    public class AvailabilityDto
    {
        public int DoctorId { get; set; }
        public string Day { get; set; }
        public double StartTimeInHours { get; set; }
        public double EndTimeInHours { get; set; }
        public string Status { get; set; } = "Available";
        //public string Username { get; set; }


    }


}

