using System;
using System.Collections.Generic;

namespace Project_Blue.Models;

public partial class Message
{
    public int IdMessage { get; set; }

    public int IdRoomchat { get; set; }

    public int? IdUser { get; set; }

    public string? NoiDung { get; set; }
}
