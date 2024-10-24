using System;
using System.Collections.Generic;

namespace CommonModule.DB;

public partial class Size
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<ProductSize> ProductSizes { get; set; } = new List<ProductSize>();
}
