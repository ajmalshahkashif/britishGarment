using System;
using System.Collections.Generic;

namespace CommonModule.DB;

public partial class Sale
{
    public int SaleId { get; set; }

    public int CustomerId { get; set; }

    public DateTime SaleDate { get; set; }

    public decimal TotalAmount { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();
}
