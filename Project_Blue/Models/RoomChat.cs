using System;
using System.Collections.Generic;

namespace Project_Blue.Models;

public partial class RoomChat
{
    public int MaPhong { get; set; }

    public string? TenPhong { get; set; }

    public string? ImageUser { get; set; }

    public string? NoiDung { get; set; }

    public int MaUser1 { get; set; }

    public int MaUser2 { get; set; }

    public virtual ThongTinCaNhan MaUser1Navigation { get; set; } = null!;

    public virtual ThongTinCaNhan MaUser2Navigation { get; set; } = null!;
}
