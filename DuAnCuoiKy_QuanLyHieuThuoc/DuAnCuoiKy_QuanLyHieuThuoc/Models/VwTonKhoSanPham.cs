using System;
using System.Collections.Generic;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Models;

public partial class VwTonKhoSanPham
{
    public string MaSp { get; set; } = null!;

    public string TenSp { get; set; } = null!;

    public string MaDvt { get; set; } = null!;

    public string? TenDvt { get; set; }

    public decimal GiaBan { get; set; }

    public int MucCanhBao { get; set; }

    public string LoaiSp { get; set; } = null!;

    public bool TrangThai { get; set; }

    public int SoLuongTon { get; set; }

    public string TrangThaiTon { get; set; } = null!;
}
