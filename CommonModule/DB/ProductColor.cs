using System;
using System.Collections.Generic;

namespace CommonModule.DB;

public partial class ProductColor
{
    public int ProductId { get; set; }

    public int ColorId { get; set; }

    public string? Description { get; set; }

    public bool IsAddedToCart { get; set; }

    public virtual Color Color { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
