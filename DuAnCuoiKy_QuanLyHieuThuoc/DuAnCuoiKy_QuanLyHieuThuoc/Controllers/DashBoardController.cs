using DuAnCuoiKy_QuanLyHieuThuoc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Controllers
{
    public class DashBoardController : Controller
    {
        public IActionResult Index_Admin()
        {
            // --- [VỊ TRÍ HARDCODE HỆ THỐNG] ---
            ViewBag.DoanhThuHomNay = "24.500.000";
            ViewBag.SoHoaDon = 142;
            ViewBag.ThuocSapHet = 12;
            ViewBag.LoSapHetHan = 05;

            ViewBag.CanhBaoTonKho = new List<dynamic>
            {
                new { Ten = "Panadol Extra", Ton = "5 hộp", Mau = "danger" },
                new { Ten = "Siro ho Prospan", Ton = "12 lọ", Mau = "warning" },
                new { Ten = "Amoxicillin 500mg", Ton = "2 vỉ", Mau = "danger" }
            };

            ViewBag.RecentInvoices = new List<dynamic>
            {
                new { MaHD = "HD-20231025-01", Gio = "10:15 - 25/10", ThuNgan = "Trần Văn Hùng", Tong = "350.000", Status = "HOÀN THÀNH", StatusClass = "success" },
                new { MaHD = "HD-20231025-02", Gio = "09:42 - 25/10", ThuNgan = "Lê Thị Lan", Tong = "1.250.000", Status = "ĐANG XỬ LÝ", StatusClass = "warning" }
            };

            return View("Index_Admin");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
