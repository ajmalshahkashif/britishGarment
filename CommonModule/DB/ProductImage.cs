﻿using System;
using System.Collections.Generic;

namespace CommonModule.DB;

public partial class ProductImage
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public byte[] Image { get; set; } = null!;

    public bool? IsMain { get; set; }

    public virtual Product Product { get; set; } = null!;
}
