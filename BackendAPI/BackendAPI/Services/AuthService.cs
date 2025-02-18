using BackendAPI.Data;
using BackendAPI.DTOs;
using BackendAPI.Helpers;
using BackendAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtTokenGenerator _tokenGenerator;

        public AuthService(ApplicationDbContext context, JwtTokenGenerator tokenGenerator)
        {
            _context = context;
            _tokenGenerator = tokenGenerator;
        }
        public async Task<AuthResult> RegisterAsync(RegisterModel model)
        {
            try
            {
                var errors = new List<string>(); 

                if (await _context.Patients.AnyAsync(p => p.Email == model.Email))
                {
                    errors.Add("Email already in use.");
                }

                var passwordRequirements = new List<string>();

                if (model.Password.Length < 6)
                {
                    passwordRequirements.Add("at least 6 characters");
                }
                if (!model.Password.Any(char.IsUpper))
                {
                    passwordRequirements.Add("at least one uppercase letter");
                }
                if (!model.Password.Any(char.IsLower))
                {
                    passwordRequirements.Add("at least one lowercase letter");
                }
                if (!model.Password.Any(char.IsDigit))
                {
                    passwordRequirements.Add("at least one number");
                }
                if (!model.Password.Any(ch => !char.IsLetterOrDigit(ch)))
                {
                    passwordRequirements.Add("at least one special character: (@?#$%^&*~)");
                }

                if (passwordRequirements.Any())
                {
                    errors.Add($"Password must contain: {string.Join(", ", passwordRequirements)}.");
                    errors.Add("For example: 'MyPassword123!' (A combination of uppercase, lowercase, numbers, and special characters).");
                }

                if (errors.Any())
                {
                    return new AuthResult { Success = false, Message = string.Join(" ", errors) };
                }

                using var transaction = await _context.Database.BeginTransactionAsync();

                var patient = new Patient
                {
                    Username = model.Username,
                    Email = model.Email,
                    Password = PasswordHasher.HashPassword(model.Password)
                };

                _context.Patients.Add(patient);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return new AuthResult { Success = true, Message = "Registration successful" };
            }
            catch (Exception ex)
            {
                return new AuthResult { Success = false, Message = $"An error occurred: {ex.Message}" };
            }
        }



        public async Task<string?> LoginAsync(LoginModel model)
        {
            try
            {
                var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Email == model.Email);
                if (patient == null)
                {
                    return "Invalid email or password.";  
                }

                if (!PasswordHasher.VerifyPassword(model.Password, patient.Password))
                {
                    return "Invalid email or password.";  
                }

                return _tokenGenerator.GenerateJwtToken(patient);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during login: {ex.Message}");
                return "An error occurred during login."; 
            }
        }

        public async Task LogoutAsync(string email, string password)
        {
            try
            {
                var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Email == email);

                if (patient == null)
                {
                    Console.WriteLine("Patient not found.");
                    return;
                }

                var passwordMatches = VerifyPassword(password, patient.Password);
                if (!passwordMatches)
                {
                    Console.WriteLine("Incorrect password.");
                    return;
                }

                var loggedOutPatient = new LoggedOut
                {
                    Email = patient.Email,
                    LogoutTime = DateTime.UtcNow
                };

                _context.LoggedOut.Add(loggedOutPatient);
                await _context.SaveChangesAsync();

                await RemoveExpiredLoggedOutPatientsAsync();

                Console.WriteLine("Patient logged out successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during logout: {ex.Message}");
            }
        }

        public async Task RemoveExpiredLoggedOutPatientsAsync()
        {
            try
            {
                var timeLimit = DateTime.UtcNow.AddDays(-60);

                var expiredPatients = await _context.LoggedOut
                                                   .Where(l => l.LogoutTime <= timeLimit)
                                                   .ToListAsync();

                if (expiredPatients.Any())
                {
                    _context.LoggedOut.RemoveRange(expiredPatients);
                    await _context.SaveChangesAsync();
                    Console.WriteLine("Expired logout records removed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during removal of expired logout records: {ex.Message}");
            }
        }

        private bool VerifyPassword(string enteredPassword, string storedPassword)
        {
            return PasswordHasher.VerifyPassword(enteredPassword, storedPassword);
        }
        public async Task<Patient?> GetPatientByEmailAsync(string email)
        {
            return await _context.Patients.FirstOrDefaultAsync(p => p.Email == email);
        }


    }
}