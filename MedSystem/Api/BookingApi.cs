using MedSystem.Data;
using MedSystem.Enums;
using MedSystem.Models;
using MedSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedSystem.Api
{
    
    public class ConfirmBookingDto
    {
        public DateTime Date { get; set; }
        public string Time { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
    }
    
    
    [Route("api/booking")]
    [ApiController]
    public class BookingApi(AvailabilityService availabilityService, 
        UserManager<ApplicationUser> userManager, 
        ApplicationDbContext context,
        INotificationService notificationService) : ControllerBase
    {
        private async Task<Patient?> GetCurrentPatientAsync()
        {
            var userId = userManager.GetUserId(User);
            if (userId == null) return null;
            
            return await context.Patients.Include(p => p.PrimaryDoctor).ThenInclude(d => d.ApplicationUser).FirstOrDefaultAsync(p => p.ApplicationUserId == userId);
        }
        
        [HttpGet("get-slots")]
        public async Task<IActionResult> GetAvailableSlots(DateTime date)
        {
            var patient = await GetCurrentPatientAsync();
            if (patient == null) return Unauthorized();

            if (date.Date < DateTime.Today)
            {
                return BadRequest("Не може да се закажува за поминат датум");
            }

            var doctorId = patient.PrimaryDoctorId.Value;
            
            var slots = await availabilityService.GetAvailableSlotsAsync(doctorId,  date);

            var result = slots.Select(s => new { time = s.ToString(@"hh\:mm") });

            return Ok(result);

        }

        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmBooking([FromBody]ConfirmBookingDto dto)
        {
            var patient = await GetCurrentPatientAsync();
            if (patient == null) return Unauthorized();
            
            
            if (!TimeSpan.TryParse(dto.Time, out var timeSpan))
                return BadRequest("Невалиден формат на време.");
            
            var appointmentDateTime = dto.Date.Date.Add(timeSpan);
            
            if (appointmentDateTime < DateTime.Now)
                return BadRequest("Не може да се закажува за минат термин.");
            
            var appointment = new Appointment
            {
                PatientId = patient.Id,
                DoctorId = patient.PrimaryDoctorId.Value,
                AppointmentDate = appointmentDateTime,
                Reason = dto.Reason,
            };
            
            context.Appointments.Add(appointment);
            await context.SaveChangesAsync();
            await notificationService.CreateAsync(
                patient.ApplicationUserId,
                $"Успешно закажан преглед. Се чека потврда од д-р.{patient.PrimaryDoctor.ApplicationUser.FirstName} {patient.PrimaryDoctor.ApplicationUser.LastName}",
                NotificationType.AppointmentPending
            );
            
            return Ok();

        }

        [HttpGet("get-appointment")]
        public async Task<IActionResult> GetAppointment(int appointmentId)
        {
            var appointment = await context.Appointments
                .Where(a => a.Id == appointmentId)
                .Select(a => new
                {
                    id = a.Id,
                    date = a.AppointmentDate,
                    status = a.Status,
                    DoctorId = a.DoctorId,
                    DoctorName = a.Doctor.ApplicationUser.FirstName + " " + a.Doctor.ApplicationUser.LastName,

                }).FirstOrDefaultAsync();
            
            if (appointment == null) return NotFound();
            return Ok(appointment);
        }

        [HttpGet("cancel")]
        public async Task<IActionResult> CancelAppointment(int id)
        {
            var appointment = await context.Appointments.FindAsync(id);

            context.Appointments.Remove(appointment);
            await  context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("approve-appointment")]
        public async Task<IActionResult> ApproveAppointment(int id)
        {
            var app = context.Appointments.Find(id);
            app.Status = Status.Confirmed;
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("reject-appointment")]
        public async Task<IActionResult> RejectAppointment(int id)
        {
            var app = context.Appointments.Find(id);
            app.Status = Status.Cancelled;
            await context.SaveChangesAsync();
            return Ok();
        }
        
       
        
    }
    
    
}
