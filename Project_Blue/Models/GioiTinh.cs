using System;
using System.Collections.Generic;

namespace Project_Blue.Models;

public partial class GioiTinh
{
    public int IdGioiTinh { get; set; }

    public string GioiTinh1 { get; set; } = null!;

    public virtual ICollection<Admin> Admins { get; set; } = new List<Admin>();

    public virtual ICollection<ThongTinCaNhan> ThongTinCaNhans { get; set; } = new List<ThongTinCaNhan>();
}
