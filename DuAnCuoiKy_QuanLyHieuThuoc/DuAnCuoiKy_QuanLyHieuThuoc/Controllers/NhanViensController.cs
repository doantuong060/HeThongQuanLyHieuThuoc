using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using DuAnCuoiKy_QuanLyHieuThuoc.Models;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Controllers
{
    public class NhanViensController : Controller
    {
        public IActionResult Index()
        {
            // --- [VỊ TRÍ HARDCODE HỆ THỐNG] ---

            // 1. Dữ liệu 4 thẻ thống kê (Ảnh 4)
            ViewBag.TongNhanSu = 24;
            ViewBag.DangLamViec = 18;
            ViewBag.DuocSiChinh = 5;
            ViewBag.NghiPhep = 3;

            // 2. Danh sách nhân viên mẫu
            var dsNhanVien = new List<dynamic>
            {
                new { Ma = "NV001", Ten = "Lê Văn An", Email = "an.le@medvault.com", GioiTinh = "Nam", SDT = "0901234567", VaiTro = "Dược sĩ trưởng", NgayVao = "15/03/2021", Status = "Hoạt động", Class = "success", Initial = "LA" },
                new { Ma = "NV002", Ten = "Trần Thị Bình", Email = "binh.tran@medvault.com", GioiTinh = "Nữ", SDT = "0912345678", VaiTro = "Nhân viên kho", NgayVao = "10/06/2022", Status = "Nghỉ phép", Class = "secondary", Initial = "TB" },
                new { Ma = "NV003", Ten = "Phạm Văn Cường", Email = "cuong.pham@medvault.com", GioiTinh = "Nam", SDT = "0987654321", VaiTro = "Bán hàng", NgayVao = "01/11/2023", Status = "Tạm nghỉ", Class = "warning", Initial = "PC" },
                new { Ma = "NV004", Ten = "Hoàng Mỹ Linh", Email = "linh.hoang@medvault.com", GioiTinh = "Nữ", SDT = "0934567890", VaiTro = "Bán hàng", NgayVao = "20/01/2024", Status = "Hoạt động", Class = "success", Initial = "HL" }
            };
            ViewBag.DanhSachNV = dsNhanVien;

            // Dữ liệu cho Dropdown Vai trò trong Modal
            ViewBag.VaiTro = new SelectList(new[] { "Quản lý", "Dược sĩ trưởng", "Bán hàng", "Nhân viên kho" });

            /* [NOTE LINQ]: 
               ViewBag.TongNhanSu = _context.NhanViens.Count();
               var nhanviens = _context.NhanViens.Include(n => n.TaiKhoan).ToList();
            */

            return View();
        }
    }
}