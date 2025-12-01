using Animal_Clinic_Project_MVC_Group_3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Animal_Clinic_Project_MVC_Group_3.Controllers
{
    public class PetClinicHController : Controller
    {
        private readonly AnimalCareClinicDbContext _context;

        public PetClinicHController(AnimalCareClinicDbContext context)
        {
            _context = context;
        }

        // GET: Pet clinic history
        public async Task<IActionResult> Index()
        {
            return View(await _context.VPetHistories.ToListAsync());

        }
    }
}
