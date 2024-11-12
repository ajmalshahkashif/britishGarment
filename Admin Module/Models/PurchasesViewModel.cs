using CommonModule.DB;
using System.ComponentModel.DataAnnotations;

namespace Admin_Module.Models
{
    public class PurchasesViewModel
    {
        public int PurchaseId { get; set; }

        [Display(Name = "Vendor")]
        public int VendorId { get; set; }
        public string? VendorName { get; set; } // Add VendorName

        [Display(Name = "Purchase Date")]
        [Required(ErrorMessage = "Please provide a purchase date.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime PurchaseDate { get; set; } = DateTime.Now;
        public int TotalAmount { get; set; }
        public int? Quantity { get; set; }
        public int? UnitPrice { get; set; }

        [Display(Name = "Category Type")]
        [Required(ErrorMessage = "Provide Category Type")]
        public int CategoryId { get; set; }

        [Display(Name = "Products")]
        public int ProductId { get; set; }
        public string? ProductName { get; set; } // Add ProductName
        public List<ProductCategory> Categories { get; set; } = new List<ProductCategory>();
        public List<Product> Products { get; set; } = new List<Product>();
        public List<User> Vendors { get; set; } = new List<User>();

        public List<PurchaseDetail> PurchaseDetails { get; set; } = new List<PurchaseDetail>();
    }

}
