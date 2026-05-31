namespace DuAnCuoiKy_QuanLyHieuThuoc.Models.ViewModels
{
    public class ProductIndexViewModel
    {
        // Thống kê
        public int TongSanPham { get; set; }
        public int SoLuongSapHet { get; set; }
        public int SoLuongSapHetHan { get; set; }

        // Danh sách hiển thị (Sử dụng View SQL để có sẵn cột SoLuongTon)
        public List<VwTonKhoSanPham> Products { get; set; } = new();
    }
}