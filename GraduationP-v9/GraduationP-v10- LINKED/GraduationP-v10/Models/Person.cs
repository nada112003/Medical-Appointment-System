using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GradutionP.Models
{
    public abstract class Person
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required, MaxLength(15)]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required, DataType(DataType.Password), MinLength(6)]
        private string PasswordHash { get; set; }

        [Required, MaxLength(200)]
        public string? Address { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
