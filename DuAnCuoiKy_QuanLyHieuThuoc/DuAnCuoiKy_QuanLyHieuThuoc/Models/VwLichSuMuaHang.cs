using System;
using System.Collections.Generic;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Models;

public partial class VwLichSuMuaHang
{
    public string? SoDienThoai { get; set; }

    public string? TenKhachHang { get; set; }

    public string MaHd { get; set; } = null!;

    public DateTime NgayBan { get; set; }

    public string TenNhanVienBan { get; set; } = null!;

    public string MaSp { get; set; } = null!;

    public string TenSp { get; set; } = null!;

    public string LoaiSp { get; set; } = null!;

    public int SoLuong { get; set; }

    public decimal DonGia { get; set; }

    public decimal? ThanhTien { get; set; }

    public string SoLo { get; set; } = null!;

    public DateOnly HanSuDung { get; set; }

    public decimal? TongTienHoaDon { get; set; }

    public string? GhiChu { get; set; }
}
