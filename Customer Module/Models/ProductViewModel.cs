using CommonModule.DB;

namespace Customer_Module.Models
{
    public class ProductViewModel
    {
        public List<Product> AllProducts { get; set; }
        public List<Product> RecentProducts { get; set; }
    }
}
