namespace GradutionP.DTO
{
    public class DoctorDto
    {
        public string Photo { get; set; } // تعديل اسم الخاصية ليكون بنفس تنسيق باقي الكود
        public string FullName { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
    }
}
