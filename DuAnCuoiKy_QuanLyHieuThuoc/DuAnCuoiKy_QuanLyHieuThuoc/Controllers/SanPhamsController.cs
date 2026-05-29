using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using DuAnCuoiKy_QuanLyHieuThuoc.Models;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Controllers
{
    // LƯU Ý 1: Phải có ": Controller" ở đây
    public class SanPhamsController : Controller
    {
        // LƯU Ý 2: Code Hardcode phải nằm TRONG hàm Index() này
        public IActionResult Index()
        {
            // --- [VỊ TRÍ HARDCODE HỆ THỐNG] ---

            // 1. Thống kê nhanh trên đầu trang
            ViewBag.TongThuoc = "1,245";
            ViewBag.SapHetHang = 48;
            ViewBag.HetHan30Ngay = 15;

            // 2. Dữ liệu danh sách thuốc (Ảnh 3)
            var dsThuoc = new List<dynamic>
            {
                new { Ma = "MED-001", Ten = "Paracetamol 500mg", HoatChat = "Paracetamol", Loai = "Giảm đau", DVT = "Vỉ", Gia = "15,000", Ton = 450, CanToa = false, Status = "success" },
                new { Ma = "MED-002", Ten = "Amoxicillin 500mg", HoatChat = "Amoxicillin trihydrate", Loai = "Kháng sinh", DVT = "Viên", Gia = "2,500", Ton = 48, CanToa = true, Status = "warning" },
                new { Ma = "MED-003", Ten = "Panadol Extra", HoatChat = "Paracetamol + Caffeine", Loai = "Giảm đau", DVT = "Hộp", Gia = "120,000", Ton = 0, CanToa = false, Status = "danger" },
                new { Ma = "MED-004", Ten = "Thuốc Mẫu 4", HoatChat = "Hoạt chất mẫu", Loai = "Loại khác", DVT = "Viên", Gia = "40,000", Ton = 140, CanToa = true, Status = "success" },
                new { Ma = "MED-005", Ten = "Thuốc Mẫu 5", HoatChat = "Hoạt chất mẫu", Loai = "Loại khác", DVT = "Viên", Gia = "50,000", Ton = 150, CanToa = false, Status = "success" }
            };
            ViewBag.DanhSachThuoc = dsThuoc;

            // Dữ liệu cho Dropdown trong Modal (Hardcode)
            ViewBag.LoaiThuoc = new SelectList(new[] { "Giảm đau", "Kháng sinh", "Thực phẩm CN", "Vật tư y tế" });
            ViewBag.DonViTinh = new SelectList(new[] { "Viên", "Vỉ", "Hộp", "Chai", "Lọ" });

            /* [NOTE LINQ TRONG TƯƠNG LAI]: 
               var thuoc = _context.SanPhams.ToList();
            */

            return View();
        }
    }
}