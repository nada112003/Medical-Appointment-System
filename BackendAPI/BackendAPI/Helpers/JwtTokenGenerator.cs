using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BackendAPI.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace BackendAPI.Helpers
{
    public class JwtTokenGenerator
    {
        private readonly string _secretKey;

        public JwtTokenGenerator(IConfiguration configuration)
        {
            _secretKey = configuration["Jwt:Secret"]!;
        }

        public string GenerateJwtToken(Patient patient)
        {
            try
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, patient.Id.ToString()),
                    new Claim(ClaimTypes.Name, patient.Username),
                    new Claim(ClaimTypes.Email, patient.Email)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: "BackendAPI",
                    audience: "BackendAPIUsers",
                    claims: claims,
                    signingCredentials: creds
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating JWT token: {ex.Message}");
                return string.Empty;
            }
        }
    }
}
