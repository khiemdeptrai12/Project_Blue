using System;
using System.Collections.Generic;

namespace Project_Blue.Models;

public partial class BanBe
{
    public int IdBanBe { get; set; }

    public int IdUser1 { get; set; }

    public int IdUser2 { get; set; }

    public int TrangThai { get; set; }

    public virtual ThongTinCaNhan IdUser1Navigation { get; set; } = null!;

    public virtual ThongTinCaNhan IdUser2Navigation { get; set; } = null!;

    public virtual TrangThaiBanBe TrangThaiNavigation { get; set; } = null!;
}
