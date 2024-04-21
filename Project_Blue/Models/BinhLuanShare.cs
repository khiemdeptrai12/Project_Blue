using System;
using System.Collections.Generic;

namespace Project_Blue.Models;

public partial class BinhLuanShare
{
    public int MaCmtShare { get; set; }

    public int MaShare { get; set; }

    public int IdUserCmtShare { get; set; }

    public string NoiDungCmtShare { get; set; } = null!;

    public virtual ThongTinCaNhan IdUserCmtShareNavigation { get; set; } = null!;
}
