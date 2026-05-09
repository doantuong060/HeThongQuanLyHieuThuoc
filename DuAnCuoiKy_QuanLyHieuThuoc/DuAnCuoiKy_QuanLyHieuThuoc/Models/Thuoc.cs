using System;
using System.Collections.Generic;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Models;

public partial class Thuoc
{
    public int MaThuoc { get; set; }

    public string TenThuoc { get; set; } = null!;

    public byte MaDvt { get; set; }

    public decimal GiaBan { get; set; }

    public int SoLuongTon { get; set; }

    public int MuongCanhBao { get; set; }

    public int MaLoai { get; set; }

    public bool CanToa { get; set; }

    public string? GhiChu { get; set; }

    public bool TrangThai { get; set; }

    public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();

    public virtual ICollection<ChiTietNhap> ChiTietNhaps { get; set; } = new List<ChiTietNhap>();

    public virtual DonViTinh MaDvtNavigation { get; set; } = null!;

    public virtual LoaiThuoc MaLoaiNavigation { get; set; } = null!;
}
