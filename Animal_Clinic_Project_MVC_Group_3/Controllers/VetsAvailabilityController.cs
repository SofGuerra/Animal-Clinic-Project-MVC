using Animal_Clinic_Project_MVC_Group_3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Animal_Clinic_Project_MVC_Group_3.Controllers
{
    public class VetsAvailabilityController : Controller
    {
        private readonly AnimalCareClinicDbContext _context;

        public VetsAvailabilityController(AnimalCareClinicDbContext context)
        {
            _context = context;
        }

        // GET: Veterinarian Availability
        public async Task<IActionResult> Index()
        {
            return View(await _context.VetsAvailabilities.ToListAsync());

        }
    }
}
