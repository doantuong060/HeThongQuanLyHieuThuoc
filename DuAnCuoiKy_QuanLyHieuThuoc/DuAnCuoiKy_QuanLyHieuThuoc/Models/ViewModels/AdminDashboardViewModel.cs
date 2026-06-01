namespace DuAnCuoiKy_QuanLyHieuThuoc.Models.ViewModels
{
    public class AdminDashboardViewModel
    {
        public decimal DoanhThuHomNay { get; set; }
        public int SoHoaDonHomNay { get; set; }
        public int ThuocSapHetHang { get; set; }
        public int LoSapHetHan { get; set; }

        // Dữ liệu cho biểu đồ (Tháng - Tiền)
        public List<string> LabelsBieuDo { get; set; } = new();
        public List<decimal> DataBieuDo { get; set; } = new();

        // Danh sách cảnh báo và hóa đơn mới nhất
        public List<VwTonKhoSanPham> DanhSachCanhBao { get; set; } = new();
        public List<VwTongTienHoaDon> HoaDonMoiNhat { get; set; } = new();
    }
}