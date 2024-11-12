using CommonModule.DB;
using System.ComponentModel.DataAnnotations;

namespace Admin_Module.Models
{
    public class SalesViewModel
    {
        public int SaleId { get; set; }
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; } // New property for User name
        public DateTime SaleDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();
        public List<User> Customer { get; set; } = new List<User>();
    }

}
