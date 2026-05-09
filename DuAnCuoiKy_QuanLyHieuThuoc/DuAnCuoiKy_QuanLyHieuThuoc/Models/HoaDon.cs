using System;
using System.Collections.Generic;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Models;

public partial class HoaDon
{
    public int MaHd { get; set; }

    public DateTime NgayBan { get; set; }

    public int MaNv { get; set; }

    public decimal TongTien { get; set; }

    public string? GhiChu { get; set; }

    public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();

    public virtual NhanVien MaNvNavigation { get; set; } = null!;
}
