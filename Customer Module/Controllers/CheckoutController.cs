using CommonModule.DB;
using Customer_Module.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Customer_Module.Controllers
{
    public class CheckoutController : BaseController
    {
        public CheckoutController(GarmentContext context) : base(context)
        {
        }

        public IActionResult CartItemsListing()
        {
            var userCart = _context.Carts
                .Where(c => c.UserId == 1) // Replace with userIdInt when session is ready
                .Select(c => new
                {
                    TotalAmount = c.TotalAmount,
                    CartItems = c.CartItems.Select(item => new CartItemViewModel
                    {
                        CartId = item.CartId,
                        ProductName = item.Product.Name,
                        Quantity = item.Quantity,
                        Price = item.Price,
                        ItemTotal = item.Quantity * item.Price,
                        Color = item.Color.Name,
                        Size = item.Size.Name,
                        ImageData = Convert.ToBase64String(item.Product.ProductImages
                                       .Where(img => img.IsMain == true)
                                       .Select(img => img.Image)
                                       .FirstOrDefault() ?? new byte[0])
                    }).ToList()
                })
                .FirstOrDefault();

            if (userCart == null)
            {
                return RedirectToAction("EmptyCart", "Checkout");
            }

            foreach (var item in userCart.CartItems)
            {
                item.GrandTotal = userCart.TotalAmount;
            }

            return View(userCart.CartItems);
        }

        [HttpPost]
        public IActionResult RemoveCartItem(int cartItemId)
        {
            var cartItem = _context.CartItems
                .Include(ci => ci.Cart)
                .FirstOrDefault(ci => ci.CartId == cartItemId);

            if (cartItem == null)
            {
                return RedirectToAction("CartItemsListing");
            }

            var cart = cartItem.Cart;


            // Remove ProductColor and ProductSize records related to this cartItem
            var productColor = _context.ProductColors
                .FirstOrDefault(pc => pc.ProductId == cartItem.ProductId && pc.ColorId == cartItem.ColorId && pc.IsAddedToCart == true);

            var productSize = _context.ProductSizes
                .FirstOrDefault(ps => ps.ProductId == cartItem.ProductId && ps.SizeId == cartItem.SizeId && ps.IsAddedToCart == true);

            //re-enable these color & size, so that they are available for user add-to-cart process
            productColor.IsAddedToCart = false;
            productSize.IsAddedToCart = false;


            // Remove the CartItem
            _context.CartItems.Remove(cartItem);
            _context.SaveChanges();

            // Check if there are any remaining items in the cart
            if (!cart.CartItems.Any())
            {
                // If no items left, mark the cart as inactive
                cart.IsActive = false;
                cart.TotalAmount = 0;
            }
            else
            {
                // Update TotalAmount for remaining items
                cart.TotalAmount = cart.CartItems.Sum(ci => ci.Quantity * ci.Price);
            }

            cart.UpdatedAt = DateTime.Now;
            _context.SaveChanges();

            var detaultCategory = _context.Products.Find(cartItem.ProductId).CategoryId;


            return RedirectToAction("ProductListing", "ProductDetails", new { categoryId = detaultCategory });
        }


        public IActionResult SelectPaymentMethod()
        {
            return View();
        }


        [HttpPost]
        public IActionResult AddSaleEntry()
        {
            // Get the current user's cart
            var cart = _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefault(c => c.UserId == 1); // Replace with session/userId logic

            if (cart == null || !cart.CartItems.Any())
            {
                // Redirect to cart if empty
                return RedirectToAction("CartItemsListing");
            }

            // Calculate the total sale amount
            var totalAmount = cart.CartItems.Sum(ci => ci.Quantity * ci.Price);

            // Create a new sale entry
            var sale = new Sale
            {
                CustomerId = 1, // Replace with session/userId logic
                SaleDate = DateTime.Now,
                TotalAmount = totalAmount,
                PaymentMethod = "Credit Card", // Replace with selected payment method logic
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            // Add the sale entry to the database
            _context.Sales.Add(sale);
            _context.SaveChanges(); // Save to get the generated SaleId

            // Create SaleDetails entries for each cart item
            foreach (var cartItem in cart.CartItems)
            {
                var saleDetail = new SaleDetail
                {
                    SaleId = sale.SaleId, // Link to the Sale
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    UnitPrice = cartItem.Price,
                    TotalPrice = cartItem.Quantity * cartItem.Price
                };
                _context.SaleDetails.Add(saleDetail);
            }

            // Update product quantities and remove ProductColor records
            foreach (var cartItem in cart.CartItems)
            {
                // Update product quantity
                var product = _context.Products.FirstOrDefault(p => p.ProductId == cartItem.ProductId);
                if (product != null)
                {
                    product.InStock -= cartItem.Quantity; // Subtract sold quantity
                    _context.Products.Update(product);
                }

                // Remove the ProductColor record as the color is now sold
                var productColor = _context.ProductColors
                    .FirstOrDefault(pc => pc.ProductId == cartItem.ProductId && pc.ColorId == cartItem.ColorId);
                if (productColor != null)
                {
                    _context.ProductColors.Remove(productColor); // Remove record from the ProductColor table
                }

                // Remove the ProductSize record as the size is now sold
                var productSize = _context.ProductSizes
                    .FirstOrDefault(pc => pc.ProductId == cartItem.ProductId && pc.SizeId== cartItem.SizeId);
                if (productSize != null)
                {
                    _context.ProductSizes.Remove(productSize); // Remove record from the ProductColor table
                }

                _context.CartItems.Remove(cartItem);
            }

            // Save changes to the database
            _context.SaveChanges();

            // After the sale is processed, clear the cart
            cart.IsActive = false;
            cart.IsPaid = true;
            cart.TotalAmount = 0;
            _context.SaveChanges();

            // Redirect to a confirmation page or order details page
            return Json(new { saleId = sale.SaleId });
        }




    }
}
