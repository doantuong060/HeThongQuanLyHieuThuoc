using System;
using System.Collections.Generic;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Models;

public partial class PhieuNhap
{
    public string MaPhieuNhap { get; set; } = null!;

    public DateTime NgayNhap { get; set; }

    public string MaNcc { get; set; } = null!;

    public string MaNv { get; set; } = null!;

    public string? GhiChu { get; set; }

    public decimal? TongTien { get; set; }

    public virtual ICollection<LoHang> LoHangs { get; set; } = new List<LoHang>();

    public virtual NhaCungCap MaNccNavigation { get; set; } = null!;

    public virtual NhanVien MaNvNavigation { get; set; } = null!;
}
