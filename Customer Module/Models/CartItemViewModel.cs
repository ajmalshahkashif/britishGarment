namespace Customer_Module.Models
{
    public class CartItemViewModel
    {
        public int CartId { get; set; }
        public string ProductName { get; set; }
        public string BrandName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal ItemTotal { get; set; }
        public decimal GrandTotal { get; set; }
        public string ImageData { get; set; } // Base64 image data
        public string Color { get; set; } // Product color selected
        public string Size { get; set; } // Product size selected

    }

}
