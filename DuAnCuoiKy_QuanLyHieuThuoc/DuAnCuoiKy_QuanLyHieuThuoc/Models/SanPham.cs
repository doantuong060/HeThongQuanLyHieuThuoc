using System;
using System.Collections.Generic;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Models;

public partial class SanPham
{
    public string MaSp { get; set; } = null!;

    public string TenSp { get; set; } = null!;

    public string MaDvt { get; set; } = null!;

    public decimal GiaBan { get; set; }

    public int MucCanhBao { get; set; }

    public string LoaiSp { get; set; } = null!;

    public bool TrangThai { get; set; }

    public virtual ICollection<LoHang> LoHangs { get; set; } = new List<LoHang>();

    public virtual DonViTinh MaDvtNavigation { get; set; } = null!;

    public virtual Thuoc? Thuoc { get; set; }

    public virtual VatTuYte? VatTuYte { get; set; }
}
