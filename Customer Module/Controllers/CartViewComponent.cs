using Microsoft.AspNetCore.Mvc;
using CommonModule.DB;
using Microsoft.EntityFrameworkCore;

namespace Customer_Module.Controllers
{
    public class CartViewComponent : ViewComponent
    {
        private readonly GarmentContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartViewComponent(GarmentContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //var userId = _httpContextAccessor.HttpContext.User.Claims
            //    .FirstOrDefault(c => c.Type == "UserId")?.Value;

            //if (string.IsNullOrEmpty(userId))
            //{
            //    return View(new CartDto()); // Return an empty cart if not logged in
            //}

            //int parsedUserId = int.Parse(userId);TODO: Uncomment this & above user check code (user session must be handled, right now it's not w
            int parsedUserId = 1;

            var cart = await _context.Carts
                  .Include(c => c.CartItems)
                      .ThenInclude(ci => ci.Product)
                      .ThenInclude(p => p.ProductImages.Where(img => img.IsMain == true))
                  .FirstOrDefaultAsync(c => c.UserId == parsedUserId && c.IsActive == true);

            var cartDto = cart != null ? new CartDto
            {
                Id = cart.Id,
                TotalAmount = cart.TotalAmount,
                Items = cart.CartItems.Select(ci => new CartItemDto
                {
                    Id = ci.ProductId,
                    Quantity = ci.Quantity,
                    Price = ci.Price,
                    productName = ci.Product.Name,
                    imageContent = ci.Product.ProductImages.FirstOrDefault()?.Image // Retrieve main image content
                }).ToList()
            } : new CartDto();

            return View(cartDto);
        }
    }
}

// DTO classes
public class CartDto
{
    public int Id { get; set; }
    public decimal? TotalAmount { get; set; }
    public List<CartItemDto> Items { get; set; }
}

public class CartItemDto
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public decimal? Price { get; set; }
    public string productName { get; set; }

    public byte[] imageContent { get; set; }


    public string Description { get; set; }
}
