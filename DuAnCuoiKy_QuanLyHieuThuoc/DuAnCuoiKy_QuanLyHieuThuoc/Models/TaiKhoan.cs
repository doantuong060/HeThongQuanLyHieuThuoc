using System;
using System.Collections.Generic;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Models;

public partial class TaiKhoan
{
    public int MaTk { get; set; }

    public string TenDangNhap { get; set; } = null!;

    public string MatKhau { get; set; } = null!;

    public DateTime NgayTao { get; set; }

    public DateTime? LanDangNhapCuoi { get; set; }

    public int MaNv { get; set; }

    public virtual NhanVien MaNvNavigation { get; set; } = null!;
}
