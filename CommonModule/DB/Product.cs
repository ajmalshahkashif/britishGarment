using System;
using System.Collections.Generic;

namespace CommonModule.DB;

public partial class Product
{
    public int ProductId { get; set; }

    public string Name { get; set; } = null!;

    public int CategoryId { get; set; }

    public decimal Price { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public virtual ProductCategory Category { get; set; } = null!;

    public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; } = new List<PurchaseDetail>();

    public virtual ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();

    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
}
