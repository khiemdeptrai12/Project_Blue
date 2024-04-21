using System;
using System.Collections.Generic;

namespace Project_Blue.Models;

public partial class ReactionSharePost
{
    public int IdReactionShare { get; set; }

    public int IdUser { get; set; }

    public int IdPostShare { get; set; }
}
