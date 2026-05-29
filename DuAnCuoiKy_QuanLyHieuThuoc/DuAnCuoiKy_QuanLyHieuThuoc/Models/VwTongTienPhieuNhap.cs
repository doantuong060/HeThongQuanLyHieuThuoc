using System;
using System.Collections.Generic;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Models;

public partial class VwTongTienPhieuNhap
{
    public string MaPhieuNhap { get; set; } = null!;

    public DateTime NgayNhap { get; set; }

    public string MaNcc { get; set; } = null!;

    public string TenNcc { get; set; } = null!;

    public string MaNv { get; set; } = null!;

    public string TenNhanVien { get; set; } = null!;

    public string? GhiChu { get; set; }

    public decimal TongTien { get; set; }
}
