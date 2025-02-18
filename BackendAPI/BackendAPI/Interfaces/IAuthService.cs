using BackendAPI.DTOs;
using BackendAPI.Models;

public interface IAuthService
{
    Task<AuthResult> RegisterAsync(RegisterModel model);
    Task<string?> LoginAsync(LoginModel model);
    Task<Patient?> GetPatientByEmailAsync(string email);
    Task LogoutAsync(string email, string password);
}
