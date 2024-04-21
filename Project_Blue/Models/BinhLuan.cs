using System;
using System.Collections.Generic;

namespace Project_Blue.Models;

public partial class BinhLuan
{
    public int MaCmt { get; set; }

    public int MaBaiPost { get; set; }

    public int IdUserCmt { get; set; }

    public string NoiDungCmt { get; set; } = null!;

    public virtual ThongTinCaNhan IdUserCmtNavigation { get; set; } = null!;
}
