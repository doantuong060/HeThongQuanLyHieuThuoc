using System;
using System.Collections.Generic;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Models;

public partial class KhachHang
{
    public string MaKh { get; set; } = null!;

    public string HoTen { get; set; } = null!;

    public string SoDienThoai { get; set; } = null!;

    public string? DiaChi { get; set; }

    public string? MaPhuongXa { get; set; }

    public string? GhiChuBenhLy { get; set; }

    public DateOnly NgayDangKy { get; set; }

    public bool TrangThai { get; set; }

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    public virtual PhuongXa? MaPhuongXaNavigation { get; set; }
}
