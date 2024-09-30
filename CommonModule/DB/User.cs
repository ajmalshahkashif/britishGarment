using System;
using System.Collections.Generic;

namespace CommonModule.DB;

public partial class User
{
    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? FirstName { get; set; }

    public string LastName { get; set; } = null!;

    public byte[] PasswordHash { get; set; } = null!;

    public byte[] PasswordSalt { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int RoleId { get; set; }

    public string? PasswordResetToken { get; set; }

    public DateTime? ResetTokenExpires { get; set; }

    public int? Test { get; set; }

    public string? Test2 { get; set; }

    public virtual Role Role { get; set; } = null!;
}
