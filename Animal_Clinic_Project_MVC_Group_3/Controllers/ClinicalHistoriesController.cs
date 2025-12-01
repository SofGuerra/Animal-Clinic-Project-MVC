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
    public class ClinicalHistoriesController : Controller
    {
        private readonly AnimalCareClinicDbContext _context;

        public ClinicalHistoriesController(AnimalCareClinicDbContext context)
        {
            _context = context;
        }

        // GET: ClinicalHistories
        public async Task<IActionResult> Index()
        {
            var animalCareClinicDbContext = _context.ClinicalHistories.Include(c => c.Pet).Include(c => c.Veterinarian);
            return View(await animalCareClinicDbContext.ToListAsync());
        }

        // GET: ClinicalHistories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ClinicalHistories == null)
            {
                return NotFound();
            }

            var clinicalHistory = await _context.ClinicalHistories
                .Include(c => c.Pet)
                .Include(c => c.Veterinarian)
                .FirstOrDefaultAsync(m => m.HistoryId == id);
            if (clinicalHistory == null)
            {
                return NotFound();
            }

            return View(clinicalHistory);
        }

        // GET: ClinicalHistories/Create
        public IActionResult Create()
        {
            ViewData["PetId"] = new SelectList(_context.Pets, "PetId", "PetId");
            ViewData["VeterinarianId"] = new SelectList(_context.Veterinarians, "UserId", "UserId");
            return View();
        }

        // POST: ClinicalHistories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HistoryId,Date,Description,Treatment,PetId,VeterinarianId,CreatedDate")] ClinicalHistory clinicalHistory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(clinicalHistory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PetId"] = new SelectList(_context.Pets, "PetId", "PetId", clinicalHistory.PetId);
            ViewData["VeterinarianId"] = new SelectList(_context.Veterinarians, "UserId", "UserId", clinicalHistory.VeterinarianId);
            return View(clinicalHistory);
        }

        // GET: ClinicalHistories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ClinicalHistories == null)
            {
                return NotFound();
            }

            var clinicalHistory = await _context.ClinicalHistories.FindAsync(id);
            if (clinicalHistory == null)
            {
                return NotFound();
            }
            ViewData["PetId"] = new SelectList(_context.Pets, "PetId", "PetId", clinicalHistory.PetId);
            ViewData["VeterinarianId"] = new SelectList(_context.Veterinarians, "UserId", "UserId", clinicalHistory.VeterinarianId);
            return View(clinicalHistory);
        }

        // POST: ClinicalHistories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HistoryId,Date,Description,Treatment,PetId,VeterinarianId,CreatedDate")] ClinicalHistory clinicalHistory)
        {
            if (id != clinicalHistory.HistoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clinicalHistory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClinicalHistoryExists(clinicalHistory.HistoryId))
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
            ViewData["PetId"] = new SelectList(_context.Pets, "PetId", "PetId", clinicalHistory.PetId);
            ViewData["VeterinarianId"] = new SelectList(_context.Veterinarians, "UserId", "UserId", clinicalHistory.VeterinarianId);
            return View(clinicalHistory);
        }

        // GET: ClinicalHistories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ClinicalHistories == null)
            {
                return NotFound();
            }

            var clinicalHistory = await _context.ClinicalHistories
                .Include(c => c.Pet)
                .Include(c => c.Veterinarian)
                .FirstOrDefaultAsync(m => m.HistoryId == id);
            if (clinicalHistory == null)
            {
                return NotFound();
            }

            return View(clinicalHistory);
        }

        // POST: ClinicalHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ClinicalHistories == null)
            {
                return Problem("Entity set 'AnimalCareClinicDbContext.ClinicalHistories'  is null.");
            }
            var clinicalHistory = await _context.ClinicalHistories.FindAsync(id);
            if (clinicalHistory != null)
            {
                _context.ClinicalHistories.Remove(clinicalHistory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClinicalHistoryExists(int id)
        {
          return (_context.ClinicalHistories?.Any(e => e.HistoryId == id)).GetValueOrDefault();
        }
    }
}
