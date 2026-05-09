using System;
using System.Collections.Generic;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Models;

public partial class VwHoaDonChiTiet
{
    public int MaHd { get; set; }

    public DateTime NgayBan { get; set; }

    public string TenNhanVien { get; set; } = null!;

    public string TenThuoc { get; set; } = null!;

    public string TenDvt { get; set; } = null!;

    public int SoLuong { get; set; }

    public decimal DonGia { get; set; }

    public decimal? ThanhTien { get; set; }

    public decimal TongTien { get; set; }

    public string? GhiChu { get; set; }
}
