using System;
using System.Collections.Generic;

namespace CommonModule.DB;

public partial class Discount
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public int? DiscountAmount { get; set; }
}
