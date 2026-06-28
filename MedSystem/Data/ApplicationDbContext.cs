using MedSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MedSystem.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<City> Cities { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<MedicalRecord> MedicalRecords { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Referral> Referrals { get; set; }
    public DbSet<Specialization> Specializations { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<City>().HasData(
            new City { Id = 1, Name = "Берово" },
            new City { Id = 2, Name = "Битола" },
            new City { Id = 3, Name = "Богданци" },
            new City { Id = 4, Name = "Валандово" },
            new City { Id = 5, Name = "Велес" },
            new City { Id = 6, Name = "Виница" },
            new City { Id = 7, Name = "Гевгелија" },
            new City { Id = 8, Name = "Гостивар" },
            new City { Id = 9, Name = "Дебар" },
            new City { Id = 10, Name = "Делчево" },
            new City { Id = 11, Name = "Демир Капија" },
            new City { Id = 12, Name = "Демир Хисар" },
            new City { Id = 13, Name = "Кавадарци" },
            new City { Id = 14, Name = "Кичево" },
            new City { Id = 15, Name = "Кочани" },
            new City { Id = 16, Name = "Кратово" },
            new City { Id = 17, Name = "Крива Паланка" },
            new City { Id = 18, Name = "Крушево" },
            new City { Id = 19, Name = "Куманово" },
            new City { Id = 20, Name = "Македонска Каменица" },
            new City { Id = 21, Name = "Македонски Брод" },
            new City { Id = 22, Name = "Неготино" },
            new City { Id = 23, Name = "Охрид" },
            new City { Id = 24, Name = "Пехчево" },
            new City { Id = 25, Name = "Прилеп" },
            new City { Id = 26, Name = "Пробиштип" },
            new City { Id = 27, Name = "Радовиш" },
            new City { Id = 28, Name = "Ресен" },
            new City { Id = 29, Name = "Свети Николе" },
            new City { Id = 30, Name = "Скопје" },
            new City { Id = 31, Name = "Струга" },
            new City { Id = 32, Name = "Струмица" },
            new City { Id = 33, Name = "Тетово" },
            new City { Id = 34, Name = "Штип" }
        );
        int skopjeCityId = 30;
        
        builder.Entity<MedicalInstitution>().HasData(
            new MedicalInstitution
                {
                    Id = 1, 
                    Name = "Универзитетски клинички центар „Мајка Тереза“",
                    Address = "Водњанска 17, Скопје 1000",
                    CityId = skopjeCityId
                },
                new MedicalInstitution
                {
                    Id = 2,
                    Name = "ГОБ „8-ми Септември“",
                    Address = "Париска бб, Тафталиџе, Скопје",
                    CityId = skopjeCityId
                },
                new MedicalInstitution
                {
                    Id = 3,
                    Name = "ЈЗУ Клиника за Универзитетска Хирургија „Св. Наум Охридски“",
                    Address = "11-ти Октомври 53, Скопје",
                    CityId = skopjeCityId
                },
                new MedicalInstitution
                {
                    Name = "Поликлиника „Букурешт“",
                    Id = 4,
                    Address = "Бледски Договор бб, Карпош 3, Скопје",
                    CityId = skopjeCityId
                },
                new MedicalInstitution
                {
                    Id = 5,
                    Name = "Поликлиника „Јане Сандански“",
                    Address = "Бул. Јане Сандански бб, Аеродром, Скопје",
                    CityId = skopjeCityId
                }
        );

        builder.Entity<Patient>(entity =>
        {
            // Eden pacient ima eden appuser i obratno
            
            entity.HasOne(p => p.ApplicationUser)
                .WithOne(u => u.Patient)
                .HasForeignKey<Patient>(p => p.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Pacientot ima embg sto e unikaten i go indeksiram

            entity.HasIndex(p => p.Embg).IsUnique();
        });

        builder.Entity<Doctor>(entity =>
        {
            // Eden doktor ima eden appuser i obratno
            
            entity.HasOne(d => d.ApplicationUser)
                .WithOne(u => u.Doctor)
                .HasForeignKey<Doctor>(d => d.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Referral>(entity =>
        {
            entity.HasOne(r => r.ReferringDoctor)
                .WithMany(d => d.IssuedReferrals)
                .HasForeignKey(r => r.ReferringDoctorId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(r => r.ReferredDoctor)
                .WithMany(d => d.ReceivedReferrals)
                .HasForeignKey(r => r.ReferredDoctorId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(r => r.ReferringSpecialization)
                .WithMany()
                .HasForeignKey(r => r.ReferringSpecializationId)
                .OnDelete(DeleteBehavior.Restrict);
            
        });


    }
}