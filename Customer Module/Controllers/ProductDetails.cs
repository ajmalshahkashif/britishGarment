using Microsoft.AspNetCore.Mvc;

namespace Customer_Module.Controllers
{
    public class ProductDetails : Controller
    {
        public IActionResult ViewProductDetails()
        {
            return View();
        }
    }
}
