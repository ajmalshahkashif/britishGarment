using System.ComponentModel.DataAnnotations;

namespace Customer_Module.Models
{
    public class ProductsViewModel
    {
        public string Name { get; set; }

        [Display(Name = "Active")]
        public bool isActive { get; set; }

        public string Description { get; set; }

        public class ColorViewModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string RgbColor { get; set; }
        }

        public class SizeViewModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
