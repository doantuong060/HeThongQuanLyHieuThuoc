using System;
using System.Collections.Generic;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Models;

public partial class LoaiVatTu
{
    public string MaLoaiVt { get; set; } = null!;

    public string TenLoaiVt { get; set; } = null!;

    public string? MoTa { get; set; }

    public virtual ICollection<VatTuYte> VatTuYtes { get; set; } = new List<VatTuYte>();
}
