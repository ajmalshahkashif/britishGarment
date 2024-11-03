using CommonModule.DB;
using System.ComponentModel.DataAnnotations;

namespace Admin_Module.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please provide product name")]
        public string Name { get; set; }


        [Display(Name = "Active")]
        public bool isActive { get; set; }

        [MaxLength(1000, ErrorMessage = "Description should not be greater not 1000 characters")]
        [Display(Name = "Short Note")]
        public string? ShortDescription { get; set; }


        [Display(Name = "Detail Description")]
        public string? DetailedDescription { get; set; }

        [Display(Name = "Pricing")]
        public decimal Price { get; set; }

        // Add this property to store the image as binary data
        [Display(Name = "Product Image")]
        public byte[]? ProductImage { get; set; }

        public string? Gender { get; set; }

        public int? Weight { get; set; }

        public int? Length { get; set; }

        public int? Height { get; set; }

        public int? InStock { get; set; }
        public int? Discount { get; set; }

        [Display(Name = "Item Type")]
        [Required(ErrorMessage = "provide Item Type")]
        public int ProductCategoryId { get; set; }

        public string? ProductCategoryName { get; set; } // Add this property for category name

        //[Display(Name = "Discount Type")]
        //public int ProductDiscountId { get; set; }

        //public string? ProductDiscountName { get; set; } // Add this property for Discount name

    }
}
