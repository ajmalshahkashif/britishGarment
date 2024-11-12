using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Admin_Module.Models;
using CommonModule.DB;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace Admin_Module.Controllers
{
    public class PurchasesController : Controller
    {
        private readonly GarmentContext _dbContext;

        public PurchasesController(GarmentContext context)
        {
            _dbContext = context;
        }

        public IActionResult AddPurchases()
        {
            LoadViewData();
            return View();
        }

        private void LoadViewData()
        {
            // Load categories for dropdown
            ViewBag.categoriesData = new SelectList(_dbContext.ProductCategories, "CategoryId", "Name");

            // Load Vendor users
            var vendorRole = _dbContext.Roles.Include(r => r.Users)
                                              .FirstOrDefault(r => r.Name == "Vendor");

            ViewBag.vendorUsers = vendorRole?.Users
                                .Select(u => new User { UserId = u.UserId, LastName = u.LastName })
                                .ToList() ?? new List<User>();
        }

        [HttpGet]
        public JsonResult SearchProducts(string searchTerm, int categoryId)
        {
            var products = _dbContext.Products
                .Where(p => p.CategoryId == categoryId && p.Name.Contains(searchTerm) && p.IsActive)
                .Select(p => new { p.ProductId, p.Name })
                .ToList();

            return Json(products);
        }

        [HttpPost]
        public IActionResult CreatePurchase(PurchasesViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Validate VendorId before saving
                    if (!_dbContext.Users.Any(u => u.UserId == model.VendorId))
                    {
                        ModelState.AddModelError("VendorId", "Selected vendor does not exist.");
                        LoadViewData();
                        return View("AllPurchases", model);
                    }

                    // Create and save Purchase
                    var purchase = new Purchase
                    {
                        VendorId = model.VendorId,
                        PurchaseDate = model.PurchaseDate,
                        TotalAmount = model.Quantity.HasValue && model.UnitPrice.HasValue ? model.Quantity.Value * model.UnitPrice.Value : 0,
                    };
                    _dbContext.Purchases.Add(purchase);
                    _dbContext.SaveChanges();

                    // Create and save PurchaseDetail
                    var purchaseDetail = new PurchaseDetail
                    {
                        PurchaseId = purchase.PurchaseId,
                        ProductId = model.ProductId,
                        Quantity = model.Quantity ?? 0,
                        UnitPrice = model.UnitPrice ?? 0
                    };
                    _dbContext.PurchaseDetails.Add(purchaseDetail);

                    // Update Product InStock and Price
                    var product = _dbContext.Products.Find(model.ProductId);
                    if (product != null)
                    {
                        product.InStock += model.Quantity ?? 0;
                        product.Price = (decimal)model.UnitPrice;
                    }
                    else
                    {
                        ModelState.AddModelError("ProductId", "The specified product does not exist.");
                        LoadViewData();
                        return View("AllPurchases", model);
                    }

                    _dbContext.SaveChanges();

                    TempData["SuccessMessage"] = "Purchase successfully created!";
                    return RedirectToAction("AllPurchases");
                }
                catch (DbUpdateException dbEx)
                {
                    ModelState.AddModelError("", "An error occurred while updating the database. Please try again.");
                    Console.WriteLine(dbEx); // Log detailed database error for debugging
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
                    Console.WriteLine(ex); // Log general error for debugging
                }
            }

            // Reload necessary data if model is invalid or an error occurs
            LoadViewData();
            return View("AllPurchases", model);
        }

        public IActionResult AllPurchases(int page = 1, int pageSize = 10)
        {
            var purchases = _dbContext.Purchases
                .Include(p => p.Vendor)
                .Include(p => p.PurchaseDetails)
                .ThenInclude(pd => pd.Product)
                .OrderBy(p => p.PurchaseDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new PurchasesViewModel
                {
                    PurchaseId = p.PurchaseId,
                    VendorId = p.VendorId,
                    VendorName = p.Vendor.LastName,  // Retrieve VendorName here
                    PurchaseDate = (DateTime)p.PurchaseDate,
                    TotalAmount = (int)p.TotalAmount,
                    PurchaseDetails = p.PurchaseDetails.Select(pd => new PurchaseDetail
                    {
                        ProductId = pd.ProductId,
                        Quantity = pd.Quantity,
                        UnitPrice = pd.UnitPrice,
                        Product = pd.Product // Including product name
                    }).ToList()
                })
                .ToList();

            int totalPurchases = _dbContext.Purchases.Count();
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPurchases = totalPurchases;

            return View(purchases);
        }

        public IActionResult ViewPurchases(int id)
        {
            var purchase = _dbContext.Purchases
        .Include(p => p.Vendor) // Assuming there is a relationship with Vendor
        .Include(p => p.PurchaseDetails)
        .ThenInclude(d => d.Product) // Assuming there is a relationship with Product
        .FirstOrDefault(p => p.PurchaseId == id);

            if (purchase == null)
            {
                return NotFound();
            }

            var model = new PurchasesViewModel
            {
                PurchaseId = purchase.PurchaseId,
                VendorId = purchase.VendorId,
                VendorName = purchase.Vendor?.LastName,
                PurchaseDate = (DateTime)purchase.PurchaseDate,
                TotalAmount = (int)purchase.TotalAmount,
                Quantity = purchase.PurchaseDetails.FirstOrDefault()?.Quantity,
                UnitPrice = (int?)(purchase.PurchaseDetails.FirstOrDefault()?.UnitPrice),
                ProductName = purchase.PurchaseDetails.FirstOrDefault()?.Product?.Name
            };

            return View(model);
        }


        // GET: Edit Purchase
        public IActionResult EditPurchases(int id)
        {
            var purchase = _dbContext.Purchases
        .Include(p => p.PurchaseDetails)
        .ThenInclude(pd => pd.Product) // Ensure Product is included
        .FirstOrDefault(p => p.PurchaseId == id);

            if (purchase == null)
            {
                TempData["ErrorMessage"] = "Purchase not found.";
                return RedirectToAction("AllPurchases");
            }
            var purchaseDetail = purchase.PurchaseDetails.FirstOrDefault();
            var model = new PurchasesViewModel
            {
                PurchaseId = purchase.PurchaseId,
                VendorId = purchase.VendorId,
                VendorName = _dbContext.Users.FirstOrDefault(u => u.UserId == purchase.VendorId)?.LastName,
                PurchaseDate = (DateTime)purchase.PurchaseDate,
                ProductId = (int)(purchase.PurchaseDetails.FirstOrDefault()?.ProductId),
                ProductName = purchaseDetail?.Product?.Name ?? "Not Available", // Set ProductName
                Quantity = purchase.PurchaseDetails.FirstOrDefault()?.Quantity,
                UnitPrice = (int?)(purchase.PurchaseDetails.FirstOrDefault()?.UnitPrice),
                TotalAmount = (int)purchase.TotalAmount,
                // Other necessary fields can be added here
            };

            LoadViewData();
            return View(model);
        }

        // POST: Edit Purchase
        [HttpPost]
        public IActionResult EditPurchases(int id, PurchasesViewModel model)
        {
            if (ModelState.IsValid)
            {
                var purchase = _dbContext.Purchases
                    .Include(p => p.PurchaseDetails)
                    .FirstOrDefault(p => p.PurchaseId == id);

                if (purchase == null)
                {
                    ModelState.AddModelError("", "Purchase not found.");
                    LoadViewData();
                    return View(model);
                }

                // Validate VendorId
                if (!_dbContext.Users.Any(u => u.UserId == model.VendorId))
                {
                    ModelState.AddModelError("VendorId", "Selected vendor does not exist.");
                    LoadViewData();
                    return View(model);
                }

                // Update purchase details
                purchase.VendorId = model.VendorId;
                purchase.PurchaseDate = model.PurchaseDate;
                purchase.TotalAmount = model.Quantity.HasValue && model.UnitPrice.HasValue
                    ? model.Quantity.Value * model.UnitPrice.Value
                    : 0;

                var purchaseDetail = purchase.PurchaseDetails.FirstOrDefault();
                var previousQuantity = purchaseDetail.Quantity;

                if (purchaseDetail != null)
                {
                    purchaseDetail.ProductId = model.ProductId;
                    purchaseDetail.Quantity = model.Quantity ?? 0;
                    purchaseDetail.UnitPrice = model.UnitPrice ?? 0;
                }

                var product = _dbContext.Products.Find(model.ProductId);
                if (product != null)
                {
                    product.InStock = product.InStock - previousQuantity + model.Quantity;
                    product.Price = (decimal)model.UnitPrice;
                }
                else
                {
                    ModelState.AddModelError("ProductId", "The specified product does not exist.");
                    LoadViewData();
                    return View("AllPurchases", model);
                }


                _dbContext.SaveChanges();

                TempData["SuccessMessage"] = "Purchase successfully updated!";
                return RedirectToAction("AllPurchases");
            }

            LoadViewData();
            return View(model);
        }
        public IActionResult DeletePurchases(int id)
        {
            var purchase = _dbContext.Purchases
        .Include(p => p.PurchaseDetails) // Include related PurchaseDetails
        .FirstOrDefault(p => p.PurchaseId == id);

            if (purchase == null)
            {
                TempData["ErrorMessage"] = "Purchase are not found!";
                return RedirectToAction("AllPurchases");
            }

            // Remove associated PurchaseDetails
            _dbContext.PurchaseDetails.RemoveRange(purchase.PurchaseDetails);

            // Remove the Purchase record
            _dbContext.Purchases.Remove(purchase);

            // Save changes to the database
            _dbContext.SaveChanges();

            TempData["SuccessMessage"] = "Purchase and Purchase Details  successfully deleted!";
            return RedirectToAction("AllPurchases");

        }
    }
}
