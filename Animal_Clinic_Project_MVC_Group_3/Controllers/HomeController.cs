using Animal_Clinic_Project_MVC_Group_3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnimalCareClinic.Controllers
{
    public class HomeController : Controller
    {
        private readonly AnimalCareClinicDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(AnimalCareClinicDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            // Get today's statistics for dashboard from all tables from database
            var today = DateTime.Today;

            var todayAppointments = await _context.Appointments
                .CountAsync(a => a.Date == today);

            var totalPets = await _context.Pets.CountAsync();
            var totalVets = await _context.Veterinarians.CountAsync();
            var completedToday = await _context.Appointments
                .CountAsync(a => a.Date == today && a.Status == "Completed");

            ViewBag.TodayAppointments = todayAppointments;
            ViewBag.TotalPets = totalPets;
            ViewBag.TotalVets = totalVets;
            ViewBag.CompletedToday = completedToday;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}