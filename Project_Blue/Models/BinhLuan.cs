using System;
using System.Collections.Generic;

namespace Project_Blue.Models;

public partial class BinhLuan
{
    public int MaCmt { get; set; }

    public int MaBaiPost { get; set; }

    public string TenNguoiCmt { get; set; } = null!;

    public string AnhNguoiCmt { get; set; } = null!;

    public string NoiDungCmt { get; set; } = null!;
}
