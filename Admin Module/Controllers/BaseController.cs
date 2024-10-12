using CommonModule.DB;
using Microsoft.AspNetCore.Mvc;

namespace Admin_Module.Controllers
{
    public class BaseController : Controller
    {
        protected readonly GarmentContext _context;

        // Constructor to inject the GarmentContext
        public BaseController(GarmentContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
