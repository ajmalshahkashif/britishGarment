using System;
using System.Collections.Generic;

namespace CommonModule.DB;

public partial class Sale
{
    public int SaleId { get; set; }

    public string CustomerName { get; set; } = null!;

    public DateTime SaleDate { get; set; }

    public decimal TotalAmount { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();
}
