using Microsoft.AspNetCore.Mvc;
using CommonModule.DB;
using Microsoft.EntityFrameworkCore;

namespace Customer_Module.Controllers
{
    public class ProductDetailsController : BaseController
    {
        public ProductDetailsController(GarmentContext context) : base(context)
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
                ps.Size.Id,
                ps.Size.Name
            }).ToList();

            ViewBag.productColor = product?.ProductColors.Select(ps => new
            {
                ps.Color.Id,
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

        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity, int sizeId, int colorId)
        {
            // Assume `User.Identity.Name` for user identification
            //int userId = int.Parse(User.Identity.Name);//TODO: uncomment it while user is handled

            int userId = 1;
            // Check if there's an active cart for this user
            var cart = _context.Carts.FirstOrDefault(c => c.UserId == userId && c.IsActive == true);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };
                _context.Carts.Add(cart);
                _context.SaveChanges();
            }

            var existingCartItem = _context.CartItems.FirstOrDefault(c => c.CartId == cart.Id && c.ProductId == productId);
            var productRecord = _context.Products.Find(productId);

            if (existingCartItem == null)
            {
                var cartItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    Quantity = quantity,
                    SizeId = sizeId,
                    ColorId = colorId,
                    Price = productRecord.Price,
                    AddedAt = DateTime.UtcNow
                };
                _context.CartItems.Add(cartItem);
            }
            else
            {
                // Item already exists, so update the quantity
                existingCartItem.Price += productRecord.Price;
                existingCartItem.Quantity += quantity;
                existingCartItem.SizeId = sizeId;
                existingCartItem.ColorId = colorId;
            }

            _context.SaveChanges();

            return Json(new { success = true, message = "Item added to cart." });
        }


    }
}
