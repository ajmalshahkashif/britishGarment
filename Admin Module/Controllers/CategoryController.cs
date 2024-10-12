using Admin_Module.Models;
using Microsoft.AspNetCore.Mvc;
using CommonModule.DB;

namespace Admin_Module.Controllers
{
    public class CategoryController : BaseController
    {
        // Constructor for CategoryController that accepts the context and passes it to the BaseController
        public CategoryController(GarmentContext context) : base(context)
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCategory(CategoryValidation category)
        {
            ProductCategory obj = new ProductCategory();

            obj.Name = category.Name;
            obj.Description = category.Description;
            obj.IsActive = category.isActive;

            _context.ProductCategories.Add(obj);
            _context.SaveChanges();
            ModelState.Clear();
            return View();
        }

        public IActionResult AllCategories()
        {
            return View();
        }
    }
}
