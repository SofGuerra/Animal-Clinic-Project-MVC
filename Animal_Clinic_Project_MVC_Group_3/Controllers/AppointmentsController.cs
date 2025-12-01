using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Animal_Clinic_Project_MVC_Group_3.Models;
using Microsoft.Data.SqlClient;
using System.Data.SqlTypes;

namespace Animal_Clinic_Project_MVC_Group_3.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly AnimalCareClinicDbContext _context;

        public AppointmentsController(AnimalCareClinicDbContext context)
        {
            _context = context;
        }

        // GET: Appointments
        public async Task<IActionResult> Index()
        {
            var animalCareClinicDbContext = _context.Appointments.Include(a => a.Pet).Include(a => a.Veterinarian);
            return View(await animalCareClinicDbContext.ToListAsync());
        }

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Appointments == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.Pet)
                .Include(a => a.Veterinarian)
                .FirstOrDefaultAsync(m => m.AppointmentId == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // GET: Appointments/Create
        public IActionResult Create()
        {
            ViewData["PetId"] = new SelectList(_context.Pets, "PetId", "PetId");
            ViewData["VeterinarianId"] = new SelectList(_context.Veterinarians, "UserId", "UserId");
            return View();
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppointmentId,Date,Time,Duration,Status,PetId,VeterinarianId")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    /*
                    int nextId = (_context.Appointments.Max(a => (int?)a.AppointmentId) ?? 0) + 1;
                    if (appointment.AppointmentId > 0)
                    {
                        bool idExists = _context.Appointments.Any(a => a.AppointmentId == appointment.AppointmentId);

                        if (!idExists)
                        {
                            nextId = appointment.AppointmentId;
                        }
                        else 
                        {
                            // Tell the user the ID is taken
                            // Then tell them the next available ID got assigned instead
                        }
                    }
                    */
                    var pId = new SqlParameter("@AppointmentID", appointment.AppointmentId);
                    var pPet = new SqlParameter("@PetID", appointment.PetId);
                    var pVet = new SqlParameter("@VeterinarianID", appointment.VeterinarianId);
                    var pDate = new SqlParameter("@Date", appointment.Date.ToString("yyyy-MM-dd"));
                    var pTime = new SqlParameter("@Time", appointment.Time.ToString(@"hh\:mm\:ss"));
                    var pDur = new SqlParameter("@Duration", appointment.Duration > 0 ? appointment.Duration : 60);

                    await _context.Database.ExecuteSqlRawAsync(
                        "EXEC sp_new_appointment @PetID, @VeterinarianID, @Date, @Time, @Duration, @AppointmentID",
                        pPet, pVet, pDate, pTime, pDur, pId);
                    return RedirectToAction(nameof(Index));
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 51000 || ex.Message.Contains("Slot unavailable"))
                    {
                        ModelState.AddModelError("", "The vet already has an appointment in this timeslot, please select a different one.");
                    }
                    else if (ex.Message.Contains("Cannot create appointments in past dates"))
                    {
                        ModelState.AddModelError("", "Database error: Cannot create at a past date.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Database error: " + ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An unnexpected error occurred: " + ex.Message);
                }
            }
            ViewData["PetId"] = new SelectList(_context.Pets, "PetId", "PetId", appointment.PetId);
            ViewData["VeterinarianId"] = new SelectList(_context.Veterinarians, "UserId", "UserId", appointment.VeterinarianId);
            return View(appointment);
        }

        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Appointments == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            ViewData["PetId"] = new SelectList(_context.Pets, "PetId", "PetId", appointment.PetId);
            ViewData["VeterinarianId"] = new SelectList(_context.Veterinarians, "UserId", "UserId", appointment.VeterinarianId);
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AppointmentId,Date,Time,Duration,Status,PetId,VeterinarianId")] Appointment appointment)
        {
            if (id != appointment.AppointmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.AppointmentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PetId"] = new SelectList(_context.Pets, "PetId", "PetId", appointment.PetId);
            ViewData["VeterinarianId"] = new SelectList(_context.Veterinarians, "UserId", "UserId", appointment.VeterinarianId);
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Appointments == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.Pet)
                .Include(a => a.Veterinarian)
                .FirstOrDefaultAsync(m => m.AppointmentId == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Appointments == null)
            {
                return Problem("Entity set 'AnimalCareClinicDbContext.Appointments'  is null.");
            }
            try
            {
                var pId = new SqlParameter("@AppointmentID", id);
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC sp_cancel_appointment @AppointmentID",
                    pId);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while deleting the appointment: " + ex.Message);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentExists(int id)
        {
          return (_context.Appointments?.Any(e => e.AppointmentId == id)).GetValueOrDefault();
        }
    }
}
