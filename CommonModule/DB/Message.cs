using System;
using System.Collections.Generic;

namespace CommonModule.DB;

public partial class Message
{
    public int MessageId { get; set; }

    public string CustomerName { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string MessageContent { get; set; } = null!;

    public DateTime SentDate { get; set; }
}
