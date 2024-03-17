using System;
using System.Collections.Generic;

namespace Project_Blue.Models;

public partial class BanBe
{
    public int MaBanBe { get; set; }

    public int MaUser1 { get; set; }

    public int MaUser2 { get; set; }

    public virtual ThongTinCaNhan MaUser1Navigation { get; set; } = null!;

    public virtual ThongTinCaNhan MaUser2Navigation { get; set; } = null!;
}
