using System;
using System.Collections.Generic;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Models;

public partial class VwTopThuocBanChay
{
    public int MaThuoc { get; set; }

    public string TenThuoc { get; set; } = null!;

    public string TenLoai { get; set; } = null!;

    public int? TongSoLuongBan { get; set; }

    public decimal? TongDoanhThu { get; set; }
}
