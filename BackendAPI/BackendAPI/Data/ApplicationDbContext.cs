using BackendAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<LoggedOut> LoggedOut { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            try
            {
                base.OnModelCreating(modelBuilder);

                modelBuilder.Ignore<Person>(); 

                modelBuilder.Entity<Patient>().ToTable("Patients");

                modelBuilder.Entity<Profile>()
                    .HasKey(p => p.PatientId);

                modelBuilder.Entity<Patient>()
                    .HasOne(p => p.Profile)
                    .WithOne(p => p.Patient)
                    .HasForeignKey<Profile>(p => p.PatientId)
                    .IsRequired();

                modelBuilder.Entity<LoggedOut>()
                    .ToTable("LoggedOutPatients")
                    .HasKey(p => p.Id); 
                modelBuilder.Entity<LoggedOut>()
                    .Property(p => p.Email)
                    .IsRequired(); 
                modelBuilder.Entity<LoggedOut>()
                    .Property(p => p.LogoutTime)
                    .IsRequired();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error configuring model: {ex.Message}");
            }
        }
    }
}
