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

            // Get all products
            var allProducts = _context.Products
                .Include(p => p.ProductImages)
                .ToList();

            // Get products created in the past 30 days
            var recentProducts = _context.Products
                .Include(p => p.ProductImages)
                .Where(p => p.CreatedAt >= DateTime.Now.AddDays(-30))
                .ToList();

            // Populate the ViewModel
            var viewModel = new ProductViewModel
            {
                AllProducts = allProducts,
                RecentProducts = recentProducts
            };

            return View(viewModel);
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
