namespace GradutionP.DTO
{
    public class AppointmentToReturnDto
    {

        public string Username { get; set; }

        public int RequestId { get; set; }
        public string? RequestsStatus { get; set; }
        public DateTime RequestedTime { get; set; }

    }
}
