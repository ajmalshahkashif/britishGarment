using Microsoft.AspNetCore.Mvc;
using CommonModule.DB;
using Microsoft.EntityFrameworkCore;
using Customer_Module.Models;

namespace Customer_Module.Controllers
{
    public class ProductDetailsController : BaseController
    {
        public ProductDetailsController(GarmentContext context) : base(context)
        {
        }

        public async Task<IActionResult> ViewProductDetails(int productId)
        {
            var product = await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductColors).ThenInclude(pc => pc.Color)
                .Include(p => p.ProductSizes).ThenInclude(ps => ps.Size)
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product == null)
            {
                return NotFound(); // Return a 404 if the product isn't found
            }

            ViewBag.productSizes = product.ProductSizes.Select(ps => new ProductsViewModel.SizeViewModel
            {
                Id = ps.Size.Id,
                Name = ps.Size.Name
            }).ToList();

            ViewBag.productColor = product.ProductColors.Select(pc => new ProductsViewModel.ColorViewModel
            {
                Id = pc.Color.Id,
                Name = pc.Color.Name,
                RgbColor = pc.Color.RgbColor
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
            var cart = _context.Carts.FirstOrDefault(c => c.UserId == userId);

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
            cart.IsActive = true;

            var CartItem = _context.CartItems.FirstOrDefault(c => c.CartId == cart.Id && c.ProductId == productId);
            var productRecord = _context.Products.Find(productId);

            if (CartItem == null)
            {
                CartItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    Quantity = quantity,
                    SizeId = sizeId,
                    ColorId = colorId,
                    Price = productRecord.Price,
                    AddedAt = DateTime.UtcNow
                };
                _context.CartItems.Add(CartItem);
            }
            else
            {
                // Item already exists, so update the quantity
                CartItem.Price += productRecord.Price;
                CartItem.Quantity += quantity;
                CartItem.SizeId = sizeId;
                CartItem.ColorId = colorId;
            }

            // Remove ProductColor and ProductSize records related to this cartItem
            var productColor = _context.ProductColors
                .FirstOrDefault(pc => pc.ProductId == CartItem.ProductId && pc.ColorId == CartItem.ColorId);
            var productSize = _context.ProductSizes
                .FirstOrDefault(ps => ps.ProductId == CartItem.ProductId && ps.SizeId == CartItem.SizeId);

            //re-enable these color & size, so that they are available for user add-to-cart process
            productColor.IsAddedToCart = true;
            productSize.IsAddedToCart = true;


            _context.SaveChanges();

            return Json(new { success = true, message = "Item added to cart." });
        }


    }
}
