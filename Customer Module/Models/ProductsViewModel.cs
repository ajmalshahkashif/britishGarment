using System.ComponentModel.DataAnnotations;

namespace Customer_Module.Models
{
    public class ProductsViewModel
    {

        public string Name { get; set; }

        [Display(Name = "Active")]
        public bool isActive { get; set; }

        public string Description { get; set; }
    }
}
