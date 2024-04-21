using System;
using System.Collections.Generic;

namespace Project_Blue.Models;

public partial class TrangThai
{
    public int IdTrangThai { get; set; }

    public string TrangThai1 { get; set; } = null!;

    public virtual ICollection<ThongTinCaNhan> ThongTinCaNhans { get; set; } = new List<ThongTinCaNhan>();
}
