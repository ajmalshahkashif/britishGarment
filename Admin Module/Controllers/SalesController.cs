using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Admin_Module.Models;
using CommonModule.DB;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace Admin_Module.Controllers
{
    public class SalesController : Controller
    {
        private readonly GarmentContext _dbContext;

        public SalesController(GarmentContext context)
        {
            _dbContext = context;
        }

        public IActionResult AllSales(int page = 1, int pageSize = 10)
        {
            var sales = _dbContext.Sales
       .Join(_dbContext.Users,
             sale => sale.CustomerId,
             user => user.UserId,
             (sale, user) => new SalesViewModel
             {
                 SaleId = sale.SaleId,
                 CustomerId = sale.CustomerId,
                 CustomerName = user.LastName, // Retrieve User name
                 SaleDate = sale.SaleDate,
                 TotalAmount = sale.TotalAmount,
                 PaymentMethod = sale.PaymentMethod,
                 CreatedAt = sale.CreatedAt,
                 UpdatedAt = sale.UpdatedAt
             })
       .OrderBy(s => s.SaleDate)
       .Skip((page - 1) * pageSize)
       .Take(pageSize)
       .ToList();

            int totalSales = _dbContext.Sales.Count();
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalSales = totalSales;

            return View(sales);
        }

        public IActionResult ViewSales(int id)
        {
            var sale = _dbContext.Sales
                .Join(_dbContext.Users,
                      sale => sale.CustomerId,
                      user => user.UserId,
                      (sale, user) => new
                      {
                          Sale = sale,
                          User = user
                      })
                .Where(joined => joined.Sale.SaleId == id)
                .Select(joined => new SalesViewModel
                {
                    SaleId = joined.Sale.SaleId,
                    CustomerId = joined.Sale.CustomerId,
                    CustomerName = joined.User.LastName, // Retrieve User name here
                    SaleDate = joined.Sale.SaleDate,
                    TotalAmount = joined.Sale.TotalAmount,
                    PaymentMethod = joined.Sale.PaymentMethod,
                    CreatedAt = joined.Sale.CreatedAt,
                    UpdatedAt = joined.Sale.UpdatedAt
                })
                .FirstOrDefault();

            if (sale == null)
            {
                return NotFound(); // Return 404 if the sale is not found
            }

            return View(sale);
        }

    }
}
