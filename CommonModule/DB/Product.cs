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

    public string? Gender { get; set; }

    public int? Weight { get; set; }

    public int? Length { get; set; }

    public int? Height { get; set; }

    public int? InStock { get; set; }

    public int? Discount { get; set; }

    public virtual ProductCategory Category { get; set; } = null!;

    public virtual ICollection<ProductColor> ProductColors { get; set; } = new List<ProductColor>();

    public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();

    public virtual ICollection<ProductSize> ProductSizes { get; set; } = new List<ProductSize>();

    public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; } = new List<PurchaseDetail>();

    public virtual ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();
}
