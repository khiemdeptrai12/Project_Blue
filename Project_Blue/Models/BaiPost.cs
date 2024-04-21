using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project_Blue.Models;

public partial class BaiPost
{
    public int MaBaiPost { get; set; }

    public string? AnhBaiPost { get; set; }

    public int MaNguoiPost { get; set; }

    [Required(ErrorMessage = "Bạn phải nhập caption!")]
    public string Caption { get; set; } = null!;

    [Required(ErrorMessage = "Bạn phải nhập mô tả!")]
    public string MoTa { get; set; } = null!;

    public int? QuantityReaction { get; set; }

    public DateTime PostedTime { get; set; }

    public virtual ThongTinCaNhan MaNguoiPostNavigation { get; set; } = null!;

    public virtual ICollection<SharePost> SharePosts { get; set; } = new List<SharePost>();
}
