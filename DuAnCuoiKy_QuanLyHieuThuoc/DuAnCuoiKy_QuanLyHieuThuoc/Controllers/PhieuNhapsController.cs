using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using DuAnCuoiKy_QuanLyHieuThuoc.Models;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Controllers
{
    public class PhieuNhapsController : Controller
    {
        public IActionResult Index()
        {
            // --- [VỊ TRÍ HARDCODE HỆ THỐNG] ---

            // Danh sách phiếu nhập mẫu
            var dsPhieu = new List<dynamic> {
                new { Ma = "PN-20231024-01", Ngay = "24/10/2023 09:30", NCC = "Công ty Dược phẩm Trung ương I", SoMatHang = 15, TongTien = "125,400,000", Status = "Đã nhập kho", Class = "success" },
                new { Ma = "PN-20231024-02", Ngay = "24/10/2023 14:15", NCC = "Nhà phân phối Thuốc Việt", SoMatHang = 8, TongTien = "45,200,000", Status = "Chờ duyệt", Class = "warning" },
                new { Ma = "PN-20231023-01", Ngay = "23/10/2023 10:00", NCC = "Dược Hậu Giang", SoMatHang = 42, TongTien = "310,500,000", Status = "Đã nhập kho", Class = "success" },
                new { Ma = "PN-20231022-03", Ngay = "22/10/2023 16:45", NCC = "Công ty TNHH Dược phẩm Đông Á", SoMatHang = 5, TongTien = "12,000,000", Status = "Đã hủy", Class = "danger" },
                new { Ma = "PN-20231021-01", Ngay = "21/10/2023 08:15", NCC = "Traphaco", SoMatHang = 20, TongTien = "89,600,000", Status = "Đã nhập kho", Class = "success" }
            };

            ViewBag.DsPhieuNhap = dsPhieu;

            // Dữ liệu cho các bộ lọc (Filter)
            ViewBag.NhaCungCap = new SelectList(new[] { "Tất cả NCC", "Dược phẩm TW1", "Dược Hậu Giang", "Traphaco" });
            ViewBag.TrangThai = new SelectList(new[] { "Tất cả trạng thái", "Đã nhập kho", "Chờ duyệt", "Đã hủy" });

            return View();
        }

        public IActionResult Create()
        {
            // --- [VỊ TRÍ HARDCODE HỆ THỐNG] ---

            // 1. Danh sách Nhà cung cấp để chọn
            ViewBag.MaNCC = new SelectList(new[] {
                new { Ma = "NCC001", Ten = "Dược phẩm Trung ương 1" },
                new { Ma = "NCC002", Ten = "Dược Hậu Giang" },
                new { Ma = "NCC003", Ten = "Traphaco" }
            }, "Ma", "Ten");

            // 2. Danh sách sản phẩm mẫu để chọn nhập
            ViewBag.DsSanPham = new List<dynamic> {
                new { Ma = "TH-0001", Ten = "Amoxicillin 500mg", DVT = "Hộp 10 vỉ x 10 viên" },
                new { Ma = "TH-0002", Ten = "Paracetamol 500mg", DVT = "Vỉ 10 viên" }
            };

            // 3. Danh sách phiếu nhập gần đây (Cột bên phải Ảnh 15)
            ViewBag.PhieuGanDay = new List<dynamic> {
                new { Ma = "PN-231024-01", NCC = "Dược phẩm TW1", Ngay = "24/10", Status = "HOÀN THÀNH" },
                new { Ma = "PN-231022-03", NCC = "Dược Hậu Giang", Ngay = "22/10", Status = "HOÀN THÀNH" }
            };

            /* [NOTE SQL]: 
               ViewBag.MaNCC = new SelectList(_context.NhaCungCaps, "MaNCC", "TenNCC");
               ViewBag.MaNV = User.FindFirst("MaNV")?.Value;
            */

            return View();
        }
    }
}