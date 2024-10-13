using Admin_Module.Models;
using Microsoft.AspNetCore.Mvc;
using CommonModule.DB;
using System.Linq;

namespace Admin_Module.Controllers
{
    public class CategoryController : BaseController
    {
        // Constructor for CategoryController that accepts the context and passes it to the BaseController
        public CategoryController(GarmentContext context) : base(context)
        {
        }

        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCategory(CategoryValidation category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ProductCategory obj = new ProductCategory
                    {
                        Name = category.Name,
                        Description = category.Description,
                        IsActive = category.isActive
                    };

                    _context.ProductCategories.Add(obj);
                    _context.SaveChanges();

                    TempData["SuccessMessage"] = "Category added successfully!";
                    return RedirectToAction("AllCategories"); // Redirect to AllCategories after adding
                }
                catch (Exception ex)
                {
                    // Log error here
                    ModelState.AddModelError("", "An error occurred while adding the category.");
                }
            }
            return View(category);
        }

        public IActionResult AllCategories(int page = 1, int pageSize = 10)
        {
            var categories = _context.ProductCategories
                .OrderBy(c => c.Name) // Optional: Order by Name or other property
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Get total count for pagination
            int totalCategories = _context.ProductCategories.Count();

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalCategories = totalCategories;

            return View(categories);
        }


        public IActionResult EditCategory(int id)
        {
            var category = _context.ProductCategories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            var categoryViewModel = new CategoryValidation
            {
                Name = category.Name,
                Description = category.Description,
                isActive = category.IsActive

            };
            return View("EditCategory", categoryViewModel);

        }

        [HttpPost]
        public IActionResult EditCategory(int id, CategoryValidation category)
        {
            if (ModelState.IsValid)
            {
                var existingCategory = _context.ProductCategories.Find(id);
                if (existingCategory != null)
                {
                    existingCategory.Name = category.Name;
                    existingCategory.Description = category.Description;
                    existingCategory.IsActive = category.isActive;

                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Category updated successfully!";
                    return RedirectToAction("AllCategories");
                }
                return NotFound();
            }
            return View(category);
        }


        public IActionResult DeleteCategory(int id)
        {
            var category = _context.ProductCategories.Find(id);
            if (category != null)
            {
                _context.ProductCategories.Remove(category);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Category deleted successfully!";
            }
            return RedirectToAction("AllCategories");
        }
    }
}
