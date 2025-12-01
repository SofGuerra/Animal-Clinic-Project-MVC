using Animal_Clinic_Project_MVC_Group_3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Animal_Clinic_Project_MVC_Group_3.Controllers
{
    public class ClinicUsersController : Controller
    {
        private readonly AnimalCareClinicDbContext _context;

        public ClinicUsersController(AnimalCareClinicDbContext context)
        {
            _context = context;
        }

        // GET: Clinic Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.ClinicUsers.ToListAsync());

        }
    }
}
