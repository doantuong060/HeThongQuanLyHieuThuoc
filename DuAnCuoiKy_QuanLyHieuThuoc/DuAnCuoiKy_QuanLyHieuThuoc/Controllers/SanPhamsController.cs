using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using DuAnCuoiKy_QuanLyHieuThuoc.Models;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Controllers
{
    public class SanPhamsController : Controller
    {
        public IActionResult Index()
        {
            // --- [VỊ TRÍ HARDCODE HỆ THỐNG] ---

            ViewBag.TongThuoc = "1,245";
            ViewBag.SapHetHang = 48;
            ViewBag.HetHan30Ngay = 15;

            // Phân loại dữ liệu: THUOC và VATTU (Khớp với SQL LoaiSP)
            var dsSanPham = new List<dynamic>
            {
                // Nhóm THUỐC
                new { Ma = "TH-0001", Ten = "Amoxicillin 500mg", PhânLoai = "THUỐC", ChiTiet = "Kháng sinh", DVT = "Viên", Gia = "4,500", Ton = 100, CanToa = true, Status = "success" },
                new { Ma = "TH-0002", Ten = "Paracetamol 500mg", PhânLoai = "THUỐC", ChiTiet = "Giảm đau", DVT = "Viên", Gia = "1,200", Ton = 45, CanToa = false, Status = "warning" },
                
                // Nhóm VẬT TƯ Y TẾ
                new { Ma = "VT-0001", Ten = "Gạc y tế 10x10cm", PhânLoai = "VẬT TƯ", ChiTiet = "Băng gạc", DVT = "Gói", Gia = "2,000", Ton = 300, CanToa = false, Status = "success" },
                new { Ma = "VT-0006", Ten = "Nhiệt kế điện tử", PhânLoai = "VẬT TƯ", ChiTiet = "Thiết bị nhỏ", DVT = "Hộp", Gia = "75,000", Ton = 5, CanToa = false, Status = "danger" },
                new { Ma = "VT-0007", Ten = "Máy đo huyết áp", PhânLoai = "VẬT TƯ", ChiTiet = "Thiết bị nhỏ", DVT = "Hộp", Gia = "450,000", Ton = 10, CanToa = false, Status = "warning" }
            };

            ViewBag.DanhSachSP = dsSanPham;
            ViewBag.LoaiSP = new SelectList(new[] { "THUỐC", "VẬT TƯ" });

            return View();
        }
    }
}