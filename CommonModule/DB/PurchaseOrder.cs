using System;
using System.Collections.Generic;

namespace CommonModule.DB;

public partial class PurchaseOrder
{
    public int PurchaseOrderId { get; set; }

    public int VendorId { get; set; }

    public DateTime OrderDate { get; set; }

    public decimal TotalAmount { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; } = new List<PurchaseOrderDetail>();

    public virtual Vendor Vendor { get; set; } = null!;
}
