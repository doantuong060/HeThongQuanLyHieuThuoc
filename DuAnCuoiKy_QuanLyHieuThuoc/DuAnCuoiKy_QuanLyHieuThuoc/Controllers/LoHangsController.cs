using Microsoft.AspNetCore.Mvc;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Controllers
{
    public class LoHangsController : Controller
    {
        public IActionResult Index()
        {
            // --- [VỊ TRÍ HARDCODE HỆ THỐNG] ---

            // 1. Thống kê tổng quan tồn kho (Cột phải Ảnh 18)
            ViewBag.TongMaThuoc = "1,248";
            ViewBag.AnToanCount = 1016;
            ViewBag.SapHetHangCount = 24;
            ViewBag.NguyCapCount = 08;

            // 2. Danh sách sản phẩm tồn kho (Bảng trái Ảnh 18)
            var dsTonKho = new List<dynamic> {
                new { Ma = "MED-001", Ten = "Panadol Extra", Loai = "Thuốc", DVT = "Hộp", Ton = 120, Nguong = 50, Progress = 100, LoGanHSD = "B-2023-X1 15/12/2025", Status = "An toàn", Class = "success" },
                new { Ma = "MED-042", Ten = "Augmentin 1g", Loai = "Kháng sinh", DVT = "Hộp", Ton = 25, Nguong = 30, Progress = 60, LoGanHSD = "C-2024-Y2 20/08/2024", Status = "Thấp", Class = "warning" },
                new { Ma = "MED-089", Ten = "Vitamin C 500mg", Loai = "Thực phẩm CN", DVT = "Lọ", Ton = 5, Nguong = 20, Progress = 20, LoGanHSD = "A-2022-Z3 05/11/2023", Status = "Sắp hết hạn", Class = "danger" }
            };

            ViewBag.DsTonKho = dsTonKho;

            /* [NOTE SQL]:
               var inventory = _context.VwTonKhoSanPhams.ToList();
               ViewBag.AnToanCount = inventory.Count(x => x.SoLuongTon > x.MucCanhBao);
            */

            return View();
        }
    }
}