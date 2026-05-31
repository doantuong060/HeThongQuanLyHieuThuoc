namespace DuAnCuoiKy_QuanLyHieuThuoc.Models.ViewModels
{
    public class ReportViewModel
    {
        // 4 Thẻ tài chính chính
        public decimal DoanhThuThang { get; set; }
        public int SoHoaDon { get; set; }
        public decimal GiaTriNhapKho { get; set; }
        public decimal LoiNhuan { get; set; }

        // Dữ liệu biểu đồ cột (12 tháng)
        public List<string> Labels12Thang { get; set; } = new();
        public List<decimal> DataDoanhThu { get; set; } = new();

        // Dữ liệu biểu đồ tròn (Cơ cấu loại thuốc)
        public List<CategoryStat> CoCauThuoc { get; set; } = new();

        // Cảnh báo (AI Monitor)
        public int SoLoSapHetHan { get; set; }
        public decimal GiaTriKhoSapHetHan { get; set; }
    }

    public class CategoryStat
    {
        public string Ten { get; set; } = "";
        public double PhanTram { get; set; }
        public string Mau { get; set; } = "";
    }
}