using DuAnCuoiKy_QuanLyHieuThuoc.Models;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Models.ViewModels
{
    public class SupplierIndexViewModel
    {
        // Danh sách 3 nhà cung cấp tiêu biểu (Hiện ở hàng Card trên cùng)
        public List<NhaCungCap> FeaturedSuppliers { get; set; } = new();

        // Toàn bộ danh sách nhà cung cấp (Hiện ở bảng dưới)
        public List<NhaCungCap> AllSuppliers { get; set; } = new();

        // Thống kê nhanh
        public int TongSoNCC { get; set; }
        public int NCCDangHoatDong { get; set; }
    }
}