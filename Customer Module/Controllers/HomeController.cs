using CommonModule.DB;
using Customer_Module.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Customer_Module.Controllers
{
    public class HomeController : BaseController
    {

        public HomeController(GarmentContext context) : base(context)
        {
        }

        public IActionResult Index()
        {
            var allProducts = _context.Products
              .Include(p => p.ProductImages)
              .Where(p => p.CreatedAt > DateTime.Now.AddDays(-30))
              .ToList();

            return View(allProducts);
        }

       

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
