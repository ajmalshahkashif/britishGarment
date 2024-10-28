using System;
using System.Collections.Generic;

namespace CommonModule.DB;

public partial class Color
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string RgbColor { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<ProductColor> ProductColors { get; set; } = new List<ProductColor>();
}
