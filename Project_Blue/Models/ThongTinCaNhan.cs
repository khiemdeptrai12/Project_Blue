using System;
using System.Collections.Generic;

namespace Project_Blue.Models;

public partial class ThongTinCaNhan
{
    public int MaKhachHang { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int TypeUser { get; set; }

    public string TenKhachHang { get; set; } = null!;

    public DateOnly? NgaySinh { get; set; }

    public string? AnhDaiDien { get; set; }

    public string? Sdt { get; set; }

    public string? TieuSu { get; set; }

    public virtual ICollection<BanBe> BanBeMaUser1Navigations { get; set; } = new List<BanBe>();

    public virtual ICollection<BanBe> BanBeMaUser2Navigations { get; set; } = new List<BanBe>();

    public virtual ICollection<RoomChat> RoomChatMaUser1Navigations { get; set; } = new List<RoomChat>();

    public virtual ICollection<RoomChat> RoomChatMaUser2Navigations { get; set; } = new List<RoomChat>();
}
