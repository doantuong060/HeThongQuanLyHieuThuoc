using System;
using System.Collections.Generic;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Models;

public partial class HoaDon
{
    public string MaHd { get; set; } = null!;

    public DateTime NgayBan { get; set; }

    public string MaNv { get; set; } = null!;

    public string? MaKh { get; set; }

    public string? GhiChu { get; set; }

    public decimal? TongTien { get; set; }

    public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();

    public virtual KhachHang? MaKhNavigation { get; set; }

    public virtual NhanVien MaNvNavigation { get; set; } = null!;
}
