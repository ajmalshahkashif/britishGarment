using Microsoft.AspNetCore.Mvc;
using CommonModule.DB;
using Microsoft.EntityFrameworkCore;

namespace Customer_Module.Controllers
{
    public class ProductDetails : BaseController
    {
        public ProductDetails(GarmentContext context) : base(context)
        {
        }

        public IActionResult ViewProductDetails(int productId)
        {
            var product = _context.Products
                  .Include(p => p.ProductImages) // Include ProductImages
                  .Include(p => p.ProductColors) // Include ProductColors for the product
                      .ThenInclude(pc => pc.Color) // Include the related Color entities
                  .Include(p => p.ProductSizes) // Include ProductSizes
                      .ThenInclude(ps => ps.Size) // Include the related Size entities
                  .FirstOrDefault(p => p.ProductId == productId);

            // Get the available sizes for the product
            ViewBag.productSizes = product?.ProductSizes.Select(ps => new
            {
                ps.SizeId,
                ps.Size.Name
            }).ToList();

            ViewBag.productColor = product?.ProductColors.Select(ps => new
            {
                ps.ColorId,
                ps.Color.RgbColor,
                ps.Color.Name
            }).ToList();

            return View(product);
        }


        public IActionResult ProductListing(int categoryId)
        {

            var allProducts = _context.Products
                .Include(p => p.ProductImages)
                .Where(p => p.CategoryId == categoryId)
                .ToList();

            return View(allProducts);

        }



    }
}
