using System;
using System.Collections.Generic;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Models;

public partial class TinhThanh
{
    public string MaTinhThanh { get; set; } = null!;

    public string TenTinhThanh { get; set; } = null!;

    public virtual ICollection<PhuongXa> PhuongXas { get; set; } = new List<PhuongXa>();
}
