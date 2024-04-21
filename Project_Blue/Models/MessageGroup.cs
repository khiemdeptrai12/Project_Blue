using System;
using System.Collections.Generic;

namespace Project_Blue.Models;

public partial class MessageGroup
{
    public int MessageId { get; set; }

    public int GroupId { get; set; }

    public int UserId { get; set; }

    public string Text { get; set; } = null!;

    public virtual GroupChat Group { get; set; } = null!;

    public virtual ThongTinCaNhan User { get; set; } = null!;
}
