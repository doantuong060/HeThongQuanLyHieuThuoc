using System;
using System.Collections.Generic;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Models;

public partial class VwTongTienHoaDon
{
    public string MaHd { get; set; } = null!;

    public DateTime NgayBan { get; set; }

    public string MaNv { get; set; } = null!;

    public string TenNhanVien { get; set; } = null!;

    public string? MaKh { get; set; }

    public string? TenKhachHang { get; set; }

    public string? GhiChu { get; set; }

    public decimal TongTien { get; set; }
}
