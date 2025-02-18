using System.Security.Cryptography;
using System.Text;

namespace BackendAPI.Helpers
{
    public static class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            try
            {
                using var sha256 = SHA256.Create();
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error hashing password: {ex.Message}");
                return string.Empty;
            }
        }

        public static bool VerifyPassword(string enteredPassword, string storedPasswordHash)
        {
            return HashPassword(enteredPassword) == storedPasswordHash;
        }
    }
}
