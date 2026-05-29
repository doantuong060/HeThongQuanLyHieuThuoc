using System;
using System.Collections.Generic;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Models;

public partial class VatTuYte
{
    public string MaSp { get; set; } = null!;

    public string MaLoaiVt { get; set; } = null!;

    public string? NhaSanXuat { get; set; }

    public virtual LoaiVatTu MaLoaiVtNavigation { get; set; } = null!;

    public virtual SanPham MaSpNavigation { get; set; } = null!;
}
