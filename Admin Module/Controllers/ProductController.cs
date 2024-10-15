using Admin_Module.Models;
using CommonModule.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Admin_Module.Controllers
{
    public class ProductController : BaseController
    {

        // Constructor for ProductController that accepts the context and passes it to the BaseController
        public ProductController(GarmentContext context) : base(context)
        {
        }

        public IActionResult AddProduct()
        {
            var productCategories = _context.ProductCategories
                .Select(x => new { x.CategoryId, x.Name })
                .ToList();

            // Use CategoryId instead of Id in SelectList
            ViewBag.productCategories = new SelectList(productCategories, "CategoryId", "Name");
            return View();
        }


        [HttpPost]
        public ActionResult AddProduct(ProductViewModel product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Product obj = new Product
                    {
                        Name = product.Name,
                        Description = product.Description,
                        Price = product.Price,
                        CategoryId = product.ProductCategoryId,
                        IsActive = product.isActive
                    };

                    _context.Products.Add(obj);
                    _context.SaveChanges();

                    TempData["SuccessMessage"] = "Product added successfully!";
                    return RedirectToAction("AllProducts"); // Redirect to AllProducts after adding
                }
                catch (Exception ex)
                {
                    // Log error here
                    ModelState.AddModelError("", "An error occurred while adding the product.");
                }
            }
            return View(product);
        }

        public IActionResult AllProducts(int page = 1, int pageSize = 10)
        {
            var products = _context.Products
                .OrderBy(c => c.Name) // Optional: Order by Name or other property
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Get total count for pagination
            int totalProductsCount = _context.Products.Count();

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.totalProductsCount = totalProductsCount;

            return View(products);
        }

        public IActionResult EditProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            // Get product categories and populate ViewBag
            var productCategories = _context.ProductCategories
                .Select(x => new { x.CategoryId, x.Name })
                .ToList();

            ViewBag.productCategories = new SelectList(productCategories, "CategoryId", "Name");

            // Prepare the ProductViewModel
            var productViewModel = new ProductViewModel
            {
                Name = product.Name,
                Description = product.Description,
                isActive = product.IsActive,
                ProductCategoryId = product.CategoryId, // Set the category ID
                Price = product.Price // Set the price
            };

            return View(productViewModel);
        }

        [HttpPost]
        public IActionResult EditProduct(int id, ProductViewModel product)
        {
            if (ModelState.IsValid)
            {
                var existingProduct = _context.Products.Find(id);
                if (existingProduct != null)
                {
                    existingProduct.Name = product.Name;
                    existingProduct.Description = product.Description;
                    existingProduct.IsActive = product.isActive;
                    existingProduct.CategoryId = product.ProductCategoryId; // Save the selected category
                    existingProduct.Price = product.Price;

                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Product updated successfully!";
                    return RedirectToAction("AllProducts");
                }
                return NotFound();
            }

            // Repopulate the ViewBag in case of validation errors
            var productCategories = _context.ProductCategories
                .Select(x => new { x.CategoryId, x.Name })
                .ToList();

            ViewBag.productCategories = new SelectList(productCategories, "CategoryId", "Name");

            return View(product);
        }

        public IActionResult ViewProduct(int id)
        {
            // Join Products with ProductCategories to get the category name
            var product = _context.Products
                .Where(p => p.ProductId == id)
                .Select(p => new ProductViewModel
                {
                    Name = p.Name,
                    Description = p.Description,
                    isActive = p.IsActive,
                    ProductCategoryId = p.CategoryId, // Set the category ID
                    ProductCategoryName = p.Category.Name, // Get the category name
                    Price = p.Price
                })
                .FirstOrDefault();

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

    }
}
