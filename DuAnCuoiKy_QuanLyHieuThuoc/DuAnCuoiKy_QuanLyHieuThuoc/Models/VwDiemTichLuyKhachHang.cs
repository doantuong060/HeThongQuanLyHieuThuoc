using System;
using System.Collections.Generic;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Models;

public partial class VwDiemTichLuyKhachHang
{
    public string MaKh { get; set; } = null!;

    public string HoTen { get; set; } = null!;

    public string SoDienThoai { get; set; } = null!;

    public string? DiaChi { get; set; }

    public string? TenPhuongXa { get; set; }

    public string? TenTinhThanh { get; set; }

    public string? GhiChuBenhLy { get; set; }

    public DateOnly NgayDangKy { get; set; }

    public bool TrangThai { get; set; }

    public decimal DiemTichLuy { get; set; }

    public decimal TongChiTieu { get; set; }
}
