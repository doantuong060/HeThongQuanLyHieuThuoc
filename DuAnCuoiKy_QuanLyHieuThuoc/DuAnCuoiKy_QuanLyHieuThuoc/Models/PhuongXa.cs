using System;
using System.Collections.Generic;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Models;

public partial class PhuongXa
{
    public string MaPhuongXa { get; set; } = null!;

    public string TenPhuongXa { get; set; } = null!;

    public string MaTinhThanh { get; set; } = null!;

    public virtual ICollection<KhachHang> KhachHangs { get; set; } = new List<KhachHang>();

    public virtual TinhThanh MaTinhThanhNavigation { get; set; } = null!;

    public virtual ICollection<NhaCungCap> NhaCungCaps { get; set; } = new List<NhaCungCap>();
}
