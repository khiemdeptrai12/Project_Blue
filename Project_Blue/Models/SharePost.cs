using System;
using System.Collections.Generic;

namespace Project_Blue.Models;

public partial class SharePost
{
    public int IdPostShare { get; set; }

    public int IdUserShare { get; set; }

    public int IdBaiPost { get; set; }

    public string? NoiDungShare { get; set; }

    public int? QuantityReactionShare { get; set; }

    public DateTime SharedTime { get; set; }

    public virtual BaiPost IdBaiPostNavigation { get; set; } = null!;

    public virtual ThongTinCaNhan IdUserShareNavigation { get; set; } = null!;
}
