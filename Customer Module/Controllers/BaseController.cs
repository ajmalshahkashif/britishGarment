using Microsoft.AspNetCore.Mvc;
using CommonModule.DB;

namespace Customer_Module.Controllers
{
    public class BaseController : Controller
    {

        protected readonly GarmentContext _context;

        // Constructor to inject the GarmentContext
        public BaseController(GarmentContext context)
        {
            _context = context;
        }
    }
}
