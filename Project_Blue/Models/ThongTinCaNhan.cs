using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project_Blue.Models;

public partial class ThongTinCaNhan
{
    public int MaKhachHang { get; set; }
    [Required(ErrorMessage = "Tài khoản không được bỏ trống!")]
    [StringLength(75, MinimumLength = 5, ErrorMessage = "Tên đăng nhập phải từ 5 đến 75 ký tự.")]
    [RegularExpression("^[a-zA-Z0-9_]*$", ErrorMessage = "Tên đăng nhập không được chứa ký tự đặc biệt.")]

    public string UserName { get; set; } = null!;
    [Required(ErrorMessage = "Mật khẩu không được bỏ trống!")]
    [StringLength(20, MinimumLength = 8, ErrorMessage = "Mật khẩu phải từ 8 đến 20 ký tự.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d])[\w\d\S]*$",
   ErrorMessage = "Mật khẩu phải chứa ít nhất một ký tự hoa, một ký tự thường và một số!")]

    public string Password { get; set; } = null!;
    [StringLength(43, ErrorMessage = "Họ Tên có tối đa 43 ký tự.")]
    public string TenKhachHang { get; set; } = null!;
    [Required(ErrorMessage = "Email là bắt buộc.")]
    [StringLength(40, MinimumLength = 6, ErrorMessage = "Email phải từ 6 đến 40 ký tự.")]
    [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")]

    public string Gmail { get; set; } = null!;

    public string AnhDaiDien { get; set; } = null!;

    public int TypeUser { get; set; }

    public DateOnly? NgaySinh { get; set; }

    public int? GioiTinh { get; set; }

    public int? TrangThai { get; set; }
    [RegularExpression(@"^\d{10}$", ErrorMessage = "Số điện thoại phải chứa đúng 10 chữ số!")]
    public string? Sdt { get; set; }

    public string? TieuSu { get; set; }

    public virtual ICollection<BaiPost> BaiPosts { get; set; } = new List<BaiPost>();

    public virtual ICollection<BanBe> BanBeIdUser1Navigations { get; set; } = new List<BanBe>();

    public virtual ICollection<BanBe> BanBeIdUser2Navigations { get; set; } = new List<BanBe>();

    public virtual ICollection<BinhLuanShare> BinhLuanShares { get; set; } = new List<BinhLuanShare>();

    public virtual ICollection<BinhLuan> BinhLuans { get; set; } = new List<BinhLuan>();

    public virtual GioiTinh? GioiTinhNavigation { get; set; }

    public virtual ICollection<GroupMember> GroupMembers { get; set; } = new List<GroupMember>();

    public virtual ICollection<MessageGroup> MessageGroups { get; set; } = new List<MessageGroup>();

    public virtual ICollection<RoomChat> RoomChatMaUser1Navigations { get; set; } = new List<RoomChat>();

    public virtual ICollection<RoomChat> RoomChatMaUser2Navigations { get; set; } = new List<RoomChat>();

    public virtual ICollection<SharePost> SharePosts { get; set; } = new List<SharePost>();

    public virtual TrangThai? TrangThaiNavigation { get; set; }
}
