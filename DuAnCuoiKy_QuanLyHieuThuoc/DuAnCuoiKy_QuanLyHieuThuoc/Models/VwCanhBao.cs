using System;
using System.Collections.Generic;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Models;

public partial class VwCanhBao
{
    public int MaThuoc { get; set; }

    public string TenThuoc { get; set; } = null!;

    public int SoLuongTon { get; set; }

    public int MuongCanhBao { get; set; }

    public string LoaiCanhBao { get; set; } = null!;

    public DateOnly? HanSuDung { get; set; }
}
