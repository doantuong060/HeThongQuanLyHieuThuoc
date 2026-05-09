using System;
using System.Collections.Generic;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Models;

public partial class ChiTietNhap
{
    public int MaCtn { get; set; }

    public int MaPhieuNhap { get; set; }

    public int MaThuoc { get; set; }

    public int SoLuong { get; set; }

    public decimal GiaNhap { get; set; }

    public string? SoLo { get; set; }

    public DateOnly? HanSuDung { get; set; }

    public decimal? ThanhTien { get; set; }

    public virtual PhieuNhap MaPhieuNhapNavigation { get; set; } = null!;

    public virtual Thuoc MaThuocNavigation { get; set; } = null!;
}
