using System;
using System.Collections.Generic;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Models;

public partial class VwDanhSachThuoc
{
    public int MaThuoc { get; set; }

    public string TenThuoc { get; set; } = null!;

    public string TenDvt { get; set; } = null!;

    public string TenLoai { get; set; } = null!;

    public decimal GiaBan { get; set; }

    public int SoLuongTon { get; set; }

    public int MuongCanhBao { get; set; }

    public string TrangThaiTon { get; set; } = null!;

    public bool CanToa { get; set; }

    public bool TrangThai { get; set; }
}
