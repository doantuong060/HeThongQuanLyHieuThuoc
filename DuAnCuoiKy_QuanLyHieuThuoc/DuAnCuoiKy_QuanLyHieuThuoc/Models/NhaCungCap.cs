using System;
using System.Collections.Generic;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Models;

public partial class NhaCungCap
{
    public string MaNcc { get; set; } = null!;

    public string TenNcc { get; set; } = null!;

    public string? DiaChi { get; set; }

    public string? MaPhuongXa { get; set; }

    public string? SoDienThoai { get; set; }

    public string? Email { get; set; }

    public bool TrangThai { get; set; }

    public virtual PhuongXa? MaPhuongXaNavigation { get; set; }

    public virtual ICollection<PhieuNhap> PhieuNhaps { get; set; } = new List<PhieuNhap>();
}
