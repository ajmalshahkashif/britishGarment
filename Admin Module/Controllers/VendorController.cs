using Admin_Module.Models;
using CommonModule.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Admin_Module.Controllers
{
    public class VendorController : Controller
    {
        private readonly GarmentContext _dbContext;

        public VendorController(GarmentContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult AddVendor()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddVendor(VendorEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Fetch the "Vendor" role by name
                var vendorRole = _dbContext.Roles.FirstOrDefault(r => r.Name == "Vendor");

                // Check if the vendor role exists
                if (vendorRole == null)
                {
                    ModelState.AddModelError("", "Vendor role not found. Please ensure the 'Vendor' role exists in the Roles table.");
                    return View("AddVendor", model);
                }

                // Create a new user with the vendor role
                var newVendorUser = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Address = model.Address,
                    MobileNo = model.MobileNo,
                    RoleId = vendorRole.RoleId,
                    CreatedAt = DateTime.Now
                };

                try
                {
                    // Save the new user to the database
                    _dbContext.Users.Add(newVendorUser);
                    _dbContext.SaveChanges();

                    TempData["SuccessMessage"] = "Vendor successfully created!";
                    return RedirectToAction("AllVendor");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while saving the vendor: " + ex.Message);
                }
            }
            else
            {
                ModelState.AddModelError("", "Please check your input. All required fields must be filled out.");
            }

            return View("AddVendor", model);
        }
        public IActionResult AllVendor(int page = 1, int pageSize = 10)
        {
            var vendors = _dbContext.Users
                .Where(u => u.Role.Name == "Vendor") // Filter by Vendor role
                .OrderBy(u => u.FirstName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Total count of vendors for pagination
            int totalVendors = _dbContext.Users.Count(u => u.Role.Name == "Vendor");

            // Set ViewBag properties with default values
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize > 0 ? pageSize : 10;  // Ensure pageSize is not zero
            ViewBag.TotalVendors = totalVendors > 0 ? totalVendors : 0; // Ensure totalVendors is not null

            return View(vendors);
        }
        public IActionResult ViewVendor(int id)
        {
            // Fetch the vendor details based on the provided ID
            var vendor = _dbContext.Users
                .Where(u => u.UserId == id && u.Role.Name == "Vendor")
                .Select(u => new VendorEditViewModel
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Address = u.Address,
                    MobileNo = u.MobileNo
                })
                .FirstOrDefault();

            if (vendor == null)
            {
                TempData["ErrorMessage"] = "Vendor not found.";
                return RedirectToAction("AllVendor");
            }

            return View(vendor);
        }
        public IActionResult EditVendor(int id)
        {
            var vendor = _dbContext.Users.Find(id);
            if (vendor == null)
            {
                TempData["ErrorMessage"] = "Vendor not found.";
                return RedirectToAction("AllVendor");
            }

            var vendorViewModel = new VendorEditViewModel
            {
                FirstName = vendor.FirstName,
                LastName = vendor.LastName,
                Email = vendor.Email,
                Address = vendor.Address,
                MobileNo = vendor.MobileNo

            };
            return View("EditVendor", vendorViewModel);
         
        }

        // POST: EditVendor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditVendor(int id, VendorEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var vendor = _dbContext.Users.Find(id);
                if (vendor != null)
                {
                    vendor.FirstName = model.FirstName;
                    vendor.LastName = model.LastName;
                    vendor.Email = model.Email;
                    vendor.Address = model.Address;
                    vendor.MobileNo = model.MobileNo;

                    _dbContext.SaveChanges();
                    TempData["SuccessMessage"] = "Vendor details updated successfully.";
                    return RedirectToAction("AllVendor");
                }
                else
                {
                    TempData["ErrorMessage"] = "Vendor not found.";
                    return RedirectToAction("AllVendor");
                }
            }

            return View(model);
        }
        public IActionResult DeleteVendor(int id)
        {
            // Find the vendor based on the provided ID
            var vendor = _dbContext.Users.Find(id);

            if (vendor == null)
            {
                TempData["ErrorMessage"] = "Vendor not found.";
                return RedirectToAction("AllVendor");
            }

            // Check if there are related Purchases for this vendor
            bool hasRelatedPurchases = _dbContext.Purchases.Any(p => p.VendorId == id);
            if (hasRelatedPurchases)
            {
                TempData["ErrorMessage"] = "Cannot delete this vendor because there are related purchases.";
                return RedirectToAction("AllVendor");
            }

            try
            {
                // Remove the vendor from the database
                _dbContext.Users.Remove(vendor);
                _dbContext.SaveChanges();
                TempData["SuccessMessage"] = "Vendor deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the vendor: " + ex.Message;
            }

            return RedirectToAction("AllVendor");
        }


    }
}
