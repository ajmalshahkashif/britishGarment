using CommonModule.DB;
using System.ComponentModel.DataAnnotations;

namespace Admin_Module.Models
{
    public class ProductEditViewModel
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
        public string? FullDescription { get; set; }

        [Display(Name = "Pricing")]
        public decimal Price { get; set; }

        // Add this property to store the image as binary data
        [Display(Name = "Product Image")]
        public byte[]? ProductImage { get; set; }

        public string? Gender { get; set; }

        [Display(Name = "Weight(kg)")]
        public int? Weight { get; set; }

        [Display(Name = "Length(cm)")]
        public int? Length { get; set; }

        [Display(Name = "Height(cm)")]
        public int? Height { get; set; }

        public int? InStock { get; set; }

        [Display(Name = "Discount %")]
        public int? Discount { get; set; }

        [Display(Name = "Item Type")]
        [Required(ErrorMessage = "provide Item Type")]
        public int ProductCategoryId { get; set; }

        public string? ProductCategoryName { get; set; } // Add this property for category name

        // Add these properties for sizes, colors, and images
        public List<int> SelectedSizes { get; set; } = new List<int>();
        public List<int> SelectedColors { get; set; } = new List<int>();
        public List<ProductImage> ProductImages { get; set; } = new List<ProductImage>(); // Assuming ProductImage is a class
    }
}
