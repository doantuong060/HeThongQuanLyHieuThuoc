using System;
using System.Collections.Generic;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Models;

public partial class Thuoc
{
    public string MaSp { get; set; } = null!;

    public string MaLoai { get; set; } = null!;

    public bool CanToa { get; set; }

    public string? GhiChu { get; set; }

    public virtual LoaiThuoc MaLoaiNavigation { get; set; } = null!;

    public virtual SanPham MaSpNavigation { get; set; } = null!;
}
