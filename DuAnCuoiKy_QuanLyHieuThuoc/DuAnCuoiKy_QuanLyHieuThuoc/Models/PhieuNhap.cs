using System;
using System.Collections.Generic;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Models;

public partial class PhieuNhap
{
    public int MaPhieuNhap { get; set; }

    public DateTime NgayNhap { get; set; }

    public int MaNcc { get; set; }

    public int MaNv { get; set; }

    public decimal TongTien { get; set; }

    public string? GhiChu { get; set; }

    public virtual ICollection<ChiTietNhap> ChiTietNhaps { get; set; } = new List<ChiTietNhap>();

    public virtual NhaCungCap MaNccNavigation { get; set; } = null!;

    public virtual NhanVien MaNvNavigation { get; set; } = null!;
}
