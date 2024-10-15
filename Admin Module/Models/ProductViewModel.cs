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
        public string Description { get; set; }

        public decimal Price { get; set; }



        [Display(Name = "Item Type")]
        [Required(ErrorMessage = "provide Item Type")]
        public int ProductCategoryId{ get; set; }
    }
}
