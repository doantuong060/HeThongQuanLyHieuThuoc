using System;
using System.Collections.Generic;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Models;

public partial class VwKiemTraHanDung
{
    public string MaSp { get; set; } = null!;

    public string TenSp { get; set; } = null!;

    public string LoaiSp { get; set; } = null!;

    public string TenDvt { get; set; } = null!;

    public string SoLo { get; set; } = null!;

    public DateOnly HanSuDung { get; set; }

    public int SoLuongConLai { get; set; }

    public int? SoNgayConLai { get; set; }

    public string TrangThaiHan { get; set; } = null!;
}
