using System;
using System.Collections.Generic;

namespace CommonModule.DB;

public partial class Purchase
{
    public int PurchaseId { get; set; }

    public int VendorId { get; set; }

    public DateTime? PurchaseDate { get; set; }

    public decimal TotalAmount { get; set; }

    public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; } = new List<PurchaseDetail>();

    public virtual User Vendor { get; set; } = null!;
}
