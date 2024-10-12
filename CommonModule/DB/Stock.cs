using System;
using System.Collections.Generic;

namespace CommonModule.DB;

public partial class Stock
{
    public int StockId { get; set; }

    public int ProductId { get; set; }

    public int QuantityAdded { get; set; }

    public DateTime StockDate { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Product Product { get; set; } = null!;
}
