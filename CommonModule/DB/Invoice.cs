using System;
using System.Collections.Generic;

namespace CommonModule.DB;

public partial class Invoice
{
    public int InvoiceId { get; set; }

    public int SaleId { get; set; }

    public DateTime InvoiceDate { get; set; }

    public decimal TotalAmount { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Sale Sale { get; set; } = null!;
}
