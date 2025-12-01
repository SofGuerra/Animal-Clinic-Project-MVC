using Animal_Clinic_Project_MVC_Group_3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Animal_Clinic_Project_MVC_Group_3.Controllers
{
    public class VetsWorkloadController : Controller
    {
        private readonly AnimalCareClinicDbContext _context;

        public VetsWorkloadController(AnimalCareClinicDbContext context)
        {
            _context = context;
        }

        // GET: Veterinarian Workload
        public async Task<IActionResult> Index()
        {
            return View(await _context.VetsWorkloads.ToListAsync());

        }
    }
}
