using DuAnCuoiKy_QuanLyHieuThuoc.Models;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Models.ViewModels
{
    public class StaffIndexViewModel
    {
        // Thống kê
        public int TongNhanSu { get; set; }
        public int DangLamViec { get; set; }
        public int DuocSiChinh { get; set; }
        public int TamNghi { get; set; }

        // Danh sách nhân viên kèm thông tin tài khoản/vai trò
        public List<NhanVien> DanhSachNhanVien { get; set; } = new();
    }
}