using System;
using System.Collections.Generic;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Models;

public partial class LoHang
{
    public string SoLo { get; set; } = null!;

    public string MaSp { get; set; } = null!;

    public string MaPhieuNhap { get; set; } = null!;

    public decimal GiaNhap { get; set; }

    public DateOnly HanSuDung { get; set; }

    public int SoLuongNhap { get; set; }

    public int SoLuongConLai { get; set; }

    public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();

    public virtual PhieuNhap MaPhieuNhapNavigation { get; set; } = null!;

    public virtual SanPham MaSpNavigation { get; set; } = null!;
}
