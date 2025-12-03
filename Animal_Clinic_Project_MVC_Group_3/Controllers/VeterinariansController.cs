using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Animal_Clinic_Project_MVC_Group_3.Models;

namespace Animal_Clinic_Project_MVC_Group_3.Controllers
{
    public class VeterinariansController : Controller
    {
        private readonly AnimalCareClinicDbContext _context;

        public VeterinariansController(AnimalCareClinicDbContext context)
        {
            _context = context;
        }

        // GET: Veterinarians
        public async Task<IActionResult> Index()
        {
              return _context.Veterinarians != null ? 
                          View(await _context.Veterinarians.ToListAsync()) :
                          Problem("Entity set 'AnimalCareClinicDbContext.Veterinarians'  is null.");
        }

        // GET: Veterinarians/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Veterinarians == null)
            {
                return NotFound();
            }

            var veterinarian = await _context.Veterinarians
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (veterinarian == null)
            {
                return NotFound();
            }

            return View(veterinarian);
        }

        // GET: Veterinarians/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Veterinarians/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,FirstName,LastName,Email,Password,Speciality,Availability,Role")] Veterinarian veterinarian)
        {
            if (ModelState.IsValid)
            {
                veterinarian.Role = "Veterinarian";
                _context.Add(veterinarian);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(veterinarian);
        }

        // GET: Veterinarians/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Veterinarians == null)
            {
                return NotFound();
            }

            var veterinarian = await _context.Veterinarians.FindAsync(id);
            if (veterinarian == null)
            {
                return NotFound();
            }
            return View(veterinarian);
        }

        // POST: Veterinarians/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,FirstName,LastName,Email,Password,Speciality,Availability,Role")] Veterinarian veterinarian)
        {
            if (id != veterinarian.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(veterinarian);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VeterinarianExists(veterinarian.UserId))
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
            return View(veterinarian);
        }

        // GET: Veterinarians/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Veterinarians == null)
            {
                return NotFound();
            }

            var veterinarian = await _context.Veterinarians
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (veterinarian == null)
            {
                return NotFound();
            }

            return View(veterinarian);
        }

        // POST: Veterinarians/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Veterinarians == null)
            {
                return Problem("Entity set 'AnimalCareClinicDbContext.Veterinarians'  is null.");
            }
            var veterinarian = await _context.Veterinarians.FindAsync(id);
            if (veterinarian != null)
            {
                _context.Veterinarians.Remove(veterinarian);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VeterinarianExists(int id)
        {
          return (_context.Veterinarians?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
