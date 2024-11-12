using Admin_Module.Models;
using CommonModule.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System;

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

            //var productDiscountType = _context.Discounts
            //    .Select(x => new { x.Id, x.Name })
            //    .ToList();

            //ViewBag.productDiscountType = new SelectList(productDiscountType, "Id", "Name");

            ViewBag.productSizes = _context.Sizes
                .Select(x => new { x.Id, x.Name })
                .ToList();

            ViewBag.productColors = _context.Colors
        .Select(x => new
        {
            x.Id,
            x.Name,
            RgbColor = "rgb(" + x.RgbColor + ")" // Format as rgb()
        })
        .ToList();

            return View();
        }

        [HttpPost]
        public ActionResult AddProduct(ProductViewModel product, string selectedSizes, string selectedColors, List<IFormFile>? ProductImages)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Create the product object and save it first
                    Product obj = new Product
                    {
                        Name = product.Name,
                        ShortDescription = product.ShortDescription,
                        FullDescription = product.DetailedDescription,
                        Price = product.Price,
                        CategoryId = product.ProductCategoryId,
                        IsActive = product.isActive,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        Gender = product.Gender,
                        Weight = product.Weight,
                        Length = product.Length,
                        Height = product.Height,
                        InStock = product.InStock,
                        Discount = product.Discount

                    };

                    _context.Products.Add(obj);
                    _context.SaveChanges(); // Save the product to get the ProductId

                    // Save the selected colors
                    if (!string.IsNullOrEmpty(selectedColors))
                    {
                        var colorIdList = selectedColors.Split(',').Select(int.Parse).ToList(); // Convert the comma-separated string into a list of integers

                        foreach (var colorId in colorIdList)
                        {
                            var productColor = new ProductColor
                            {
                                ProductId = obj.ProductId,  // Use the saved product's ID
                                ColorId = colorId
                            };

                            _context.ProductColors.Add(productColor);
                        }

                        _context.SaveChanges(); // Save all selected colors
                    }

                    // Save the selected sizes
                    if (!string.IsNullOrEmpty(selectedSizes))
                    {
                        var sizeIdList = selectedSizes.Split(',').Select(int.Parse).ToList(); // Split the comma-separated sizes and convert to list of integers

                        foreach (var sizeId in sizeIdList)
                        {
                            var productSize = new ProductSize
                            {
                                ProductId = obj.ProductId,  // Use the saved product's ID
                                SizeId = sizeId
                            };

                            _context.ProductSizes.Add(productSize);
                        }

                        _context.SaveChanges(); // Save all selected sizes
                    }
                    // Now save the multiple images, if any
                    if (ProductImages != null && ProductImages.Count > 0)
                    {
                        foreach (var image in ProductImages)
                        {
                            if (image.Length > 0)
                            {
                                using (var memoryStream = new MemoryStream())
                                {
                                    image.CopyTo(memoryStream);
                                    var imageData = memoryStream.ToArray();

                                    ProductImage productImage = new ProductImage
                                    {
                                        ProductId = obj.ProductId, // Set the ProductId from the saved product
                                        Image = imageData
                                    };

                                    _context.ProductImages.Add(productImage);
                                }
                            }
                        }

                        _context.SaveChanges(); // Save all images in the database
                    }

                    TempData["SuccessMessage"] = "Product added successfully!";
                    return RedirectToAction("AllProducts"); // Redirect to AllProducts after adding
                }
                catch (Exception ex)
                {
                    // Log error here
                    ModelState.AddModelError("", "An error occurred while adding the product.");
                }
            }

            // Re-populate the ViewBag in case of validation errors
            var productCategories = _context.ProductCategories
                .Select(x => new { x.CategoryId, x.Name })
                .ToList();
            ViewBag.productCategories = new SelectList(productCategories, "CategoryId", "Name");

            return View(product);
        }


        public IActionResult AllProducts(int page = 1, int pageSize = 10, string searchTerm = "", int? categoryId = null)
        {
            var productsQuery = _context.Products.AsQueryable();

            // Filter by search term
            if (!string.IsNullOrEmpty(searchTerm))
            {
                productsQuery = productsQuery.Where(p => p.Name.Contains(searchTerm));
            }

            // Filter by category
            if (categoryId.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.CategoryId == categoryId.Value);
            }

            // Pagination
            var products = productsQuery
                .OrderBy(p => p.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            int totalProductsCount = productsQuery.Count();

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.totalProductsCount = totalProductsCount;

            // Pass active categories to ViewBag
            ViewBag.productCategories = new SelectList(_context.ProductCategories.Where(c => c.IsActive), "CategoryId", "Name");
            ViewBag.SearchTerm = searchTerm;
            ViewBag.SelectedCategoryId = categoryId;

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

            // Get available sizes and colors
            ViewBag.productSizes = _context.Sizes.Select(x => new { x.Id, x.Name }).ToList();
            ViewBag.productColors = _context.Colors
                .Select(x => new { x.Id, x.Name, RgbColor = "rgb(" + x.RgbColor + ")" })
                .ToList();

            // Get selected sizes and colors for this product
            var selectedSizes = _context.ProductSizes
                .Where(ps => ps.ProductId == id)
                .Select(ps => ps.SizeId)
                .ToList();
            var selectedColors = _context.ProductColors
                .Where(pc => pc.ProductId == id)
                .Select(pc => pc.ColorId)
                .ToList();
            // Prepare the ProductEditViewModel
            var productEditViewModel = new ProductEditViewModel
            {
                Name = product.Name,
                ShortDescription = product.ShortDescription,
                FullDescription = product.FullDescription,
                isActive = product.IsActive,
                ProductCategoryId = product.CategoryId, // Set the category ID
                Price = product.Price, // Set the price
                Gender = product.Gender,
                Weight = product.Weight,
                Length = product.Length,
                Height = product.Height,
                InStock = product.InStock,
                Discount = product.Discount,
                SelectedSizes = selectedSizes,   // Add selected sizes
                SelectedColors = selectedColors, // Add selected colors
                ProductImages = _context.ProductImages.Where(pi => pi.ProductId == id).ToList() // Fetch existing images
            };



            return View(productEditViewModel);
        }

        [HttpPost]
        public IActionResult EditProduct(int id, ProductViewModel product, string selectedSizes, string selectedColors, List<IFormFile>? ProductImages)
        {
            if (ModelState.IsValid)
            {
                var existingProduct = _context.Products.Find(id);
                if (existingProduct != null)
                {
                    existingProduct.Name = product.Name;
                    existingProduct.FullDescription = product.DetailedDescription;
                    existingProduct.ShortDescription = product.ShortDescription;
                    existingProduct.IsActive = product.isActive;
                    existingProduct.CategoryId = product.ProductCategoryId; // Save the selected category
                    existingProduct.Price = product.Price;
                    existingProduct.Gender = product.Gender;
                    existingProduct.Weight = product.Weight;
                    existingProduct.Length = product.Length;
                    existingProduct.Height = product.Height;
                    existingProduct.InStock = product.InStock;
                    existingProduct.Discount = product.Discount;


                    // Update product colors
                    var existingColors = _context.ProductColors.Where(pc => pc.ProductId == id).ToList();
                    _context.ProductColors.RemoveRange(existingColors); // Remove old selections
                    if (!string.IsNullOrEmpty(selectedColors))
                    {
                        var colorIdList = selectedColors.Split(',').Select(int.Parse).ToList();
                        foreach (var colorId in colorIdList)
                        {
                            _context.ProductColors.Add(new ProductColor { ProductId = id, ColorId = colorId });
                        }
                    }

                    // Update product sizes
                    var existingSizes = _context.ProductSizes.Where(ps => ps.ProductId == id).ToList();
                    _context.ProductSizes.RemoveRange(existingSizes); // Remove old selections
                    if (!string.IsNullOrEmpty(selectedSizes))
                    {
                        var sizeIdList = selectedSizes.Split(',').Select(int.Parse).ToList();
                        foreach (var sizeId in sizeIdList)
                        {
                            _context.ProductSizes.Add(new ProductSize { ProductId = id, SizeId = sizeId });
                        }
                    }

                    // Update product images
                    if (ProductImages != null && ProductImages.Count > 0)
                    {
                        foreach (var image in ProductImages)
                        {
                            if (image.Length > 0)
                            {
                                using (var memoryStream = new MemoryStream())
                                {
                                    image.CopyTo(memoryStream);
                                    var imageData = memoryStream.ToArray();

                                    // Create new ProductImage instance and add to context
                                    ProductImage productImage = new ProductImage
                                    {
                                        ProductId = id,
                                        Image = imageData
                                    };

                                    _context.ProductImages.Add(productImage);
                                }
                            }
                        }
                    }


                    _context.SaveChanges();
                    // Set a success message
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
            ViewBag.productSizes = _context.Sizes.Select(x => new { x.Id, x.Name }).ToList();

            // Get selected size names for this product
            var selectedSizeNames = _context.ProductSizes
                .Where(ps => ps.ProductId == id)
                .Join(_context.Sizes, ps => ps.SizeId, s => s.Id, (ps, s) => s.Name)
                .ToList();
            // Get selected color names for this product
            var selectedColorNames = _context.ProductColors
                .Where(pc => pc.ProductId == id)
                .Join(_context.Colors, pc => pc.ColorId, c => c.Id, (pc, c) => c.Name)
                .ToList();
            var product = _context.Products
                .Where(p => p.ProductId == id)
                .Select(p => new ProductEditViewModel
                {
                    Name = p.Name,
                    ShortDescription = p.ShortDescription,
                    FullDescription = p.FullDescription,
                    isActive = p.IsActive,
                    ProductCategoryId = p.CategoryId, // Set the category ID
                    ProductCategoryName = p.Category.Name, // Get the category name
                    Price = p.Price,
                    Gender = p.Gender,
                    Weight = p.Weight,
                    Length = p.Length,
                    Height = p.Height,
                    InStock = p.InStock,
                    Discount = p.Discount,
                    SelectedSizes = _context.ProductSizes
                            .Where(ps => ps.ProductId == id)
                            .Select(ps => ps.SizeId).ToList(), // Keep SelectedSizes as is
                    SelectedColors = _context.ProductColors
                             .Where(pc => pc.ProductId == id)
                             .Select(pc => pc.ColorId).ToList(), // Keep SelectedColors
                    ProductImages = _context.ProductImages.Where(pi => pi.ProductId == id).ToList() // Fetch existing images
                })
                .FirstOrDefault();

            if (product == null)
            {
                return NotFound();
            }
            ViewBag.SelectedSizeNames = selectedSizeNames; // Pass size names through ViewBag
            ViewBag.SelectedColorNames = selectedColorNames; // Pass color names through ViewBag
            return View(product);
        }
        public IActionResult DeleteProduct(int id)
        {
            // Find the product by id
            var product = _context.Products
                .Include(p => p.ProductImages) // Include related images
                .Include(p => p.ProductSizes) // Include related sizes
                .Include(p => p.ProductColors) // Include related colors
              .FirstOrDefault(p => p.ProductId == id);

            if (product != null)
            {
                // Remove associated product images
                _context.ProductImages.RemoveRange(product.ProductImages);

                // Remove associated product sizes
                _context.ProductSizes.RemoveRange(product.ProductSizes);

                // Remove associated product colors
                _context.ProductColors.RemoveRange(product.ProductColors);

                // Now remove the product
                _context.Products.Remove(product);

                // Save changes to the database
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Product And Its Related Data Deleted Successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Product not found!";
            }
            return RedirectToAction("AllProducts");
        }
    }
}
