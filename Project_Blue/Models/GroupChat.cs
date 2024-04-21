using System;
using System.Collections.Generic;

namespace Project_Blue.Models;

public partial class GroupChat
{
    public int GroupId { get; set; }

    public string GroupName { get; set; } = null!;

    public string? GroupImage { get; set; }

    public virtual ICollection<GroupMember> GroupMembers { get; set; } = new List<GroupMember>();

    public virtual ICollection<MessageGroup> MessageGroups { get; set; } = new List<MessageGroup>();
}
