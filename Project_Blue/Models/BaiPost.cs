using System;
using System.Collections.Generic;

namespace Project_Blue.Models;

public partial class BaiPost
{
    public int MaBaiPost { get; set; }

    public string? AnhBaiPost { get; set; }

    public int MaNguoiPost { get; set; }

    public string Caption { get; set; } = null!;

    public string MoTa { get; set; } = null!;

    public int? QuantityReaction { get; set; }

    public virtual ThongTinCaNhan MaNguoiPostNavigation { get; set; } = null!;
}
