using System;
using System.Collections.Generic;

namespace Project_Blue.Models;

public partial class ReactionPost
{
    public int IdReaction { get; set; }

    public int IdUser { get; set; }

    public int IdPost { get; set; }
}
