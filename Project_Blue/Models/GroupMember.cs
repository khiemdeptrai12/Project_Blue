using System;
using System.Collections.Generic;

namespace Project_Blue.Models;

public partial class GroupMember
{
    public int GroupMemberId { get; set; }

    public int GroupId { get; set; }

    public int UserId { get; set; }

    public DateTime CreateAt { get; set; }

    public virtual GroupChat Group { get; set; } = null!;

    public virtual ThongTinCaNhan User { get; set; } = null!;
}
