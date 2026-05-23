using System;
using System.Collections.Generic;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Models;

public partial class ChiTietHoaDon
{
    public string MaCthd { get; set; } = null!;

    public string MaHd { get; set; } = null!;

    public string SoLo { get; set; } = null!;

    public int SoLuong { get; set; }

    public decimal DonGia { get; set; }

    public virtual HoaDon MaHdNavigation { get; set; } = null!;

    public virtual LoHang SoLoNavigation { get; set; } = null!;
}
