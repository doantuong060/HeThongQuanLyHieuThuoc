using System;
using System.Collections.Generic;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Models.ViewModels
{
    public class TongQuanKhoViewModel
    {
        public int TongMatHang { get; set; }

        public int SapHetHang { get; set; }

        public int SapHetHan { get; set; }

        public List<ThuocCanNhapVM> DsCanNhap { get; set; } = new();

        public List<LoSapHetHanVM> DsHetHan { get; set; } = new();

        public List<PhieuNhapGanDayVM> DsPhieuNhap { get; set; } = new();
    }

    public class ThuocCanNhapVM
    {
        public string TenThuoc { get; set; } = "";

        public int SoLuongTon { get; set; }

        public int MucCanhBao { get; set; }

        public string DonViTinh { get; set; } = "";
    }

    public class LoSapHetHanVM
    {
        public string TenThuoc { get; set; } = "";

        public string SoLo { get; set; } = "";

        public DateOnly HanSuDung { get; set; }

        public int SoNgayConLai { get; set; }

        public string TrangThai { get; set; } = "";

        public string CssClass { get; set; } = "";
    }

    public class PhieuNhapGanDayVM
    {
        public string MaPhieuNhap { get; set; } = "";

        public DateTime NgayNhap { get; set; }

        public string NhaCungCap { get; set; } = "";

        public decimal TongTien { get; set; }
    }
}