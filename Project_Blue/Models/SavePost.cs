using System;
using System.Collections.Generic;

namespace Project_Blue.Models;

public partial class SavePost
{
    public int IdSave { get; set; }

    public int IdPost { get; set; }

    public int IdUserSave { get; set; }

    public int LoaiPost { get; set; }
}
