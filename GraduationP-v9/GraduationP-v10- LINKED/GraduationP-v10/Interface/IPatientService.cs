using GradutionP.DTO;
using GradutionP.Models;

namespace GradutionP.Interface
{
    public interface IPatientService
    {
        /// <summary>
        /// استرجاع بروفايل الطبيب بالتفاصيل
        /// </summary>
        /// <param name="doctorId">رقم تعريف الطبيب</param>
        /// <returns>كائن يحتوي على بيانات بروفايل الطبيب</returns>
        Task<DoctorProfile> GetDoctorProfileAsync(int doctorId);

        /// <summary>
        /// البحث عن الأطباء بناءً على استعلام المستخدم
        /// </summary>
        /// <param name="searchDto">كائن يحتوي على بيانات البحث</param>
        /// <returns>قائمة بالأطباء المطابقين</returns>
      //  Task<List<DoctorDto>> SearchDoctorsAsync(DoctorSearchDto searchDto);

        Task<List<object>> GetDoctorAppointmentsAsync(int doctorId);
        Task<Appointment> BookAppointmentAsync(int doctorId, int patientId, int availabilityId);//لتجنب الخطأ البشرى
        Task<object> GetAppointmentsByStatusAsync(int patientId, string? status = null);

        Task<bool> CancelAppointmentAsync(int appointmentId);

    }
}
