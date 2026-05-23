using System;
using System.Collections.Generic;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Models;

public partial class NhanVien
{
    public string MaNv { get; set; } = null!;

    public string HoTen { get; set; } = null!;

    public string GioiTinh { get; set; } = null!;

    public DateOnly NgaySinh { get; set; }

    public string SoDienThoai { get; set; } = null!;

    public string? Email { get; set; }

    public DateOnly NgayVaoLam { get; set; }

    public bool TrangThai { get; set; }

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    public virtual ICollection<PhieuNhap> PhieuNhaps { get; set; } = new List<PhieuNhap>();

    public virtual TaiKhoan? TaiKhoan { get; set; }
}
