using System;
using System.Collections.Generic;

namespace Project_Blue.Models;

public partial class TrangThaiBanBe
{
    public int IdtrangThai { get; set; }

    public string TenTrangThai { get; set; } = null!;

    public virtual ICollection<BanBe> BanBes { get; set; } = new List<BanBe>();
}
