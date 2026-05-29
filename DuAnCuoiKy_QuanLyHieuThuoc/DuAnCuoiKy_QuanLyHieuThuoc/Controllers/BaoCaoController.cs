using Microsoft.AspNetCore.Mvc;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Controllers
{
    public class BaoCaoController : Controller
    {
        public IActionResult Index()
        {
            // --- [VỊ TRÍ HARDCODE HỆ THỐNG] ---

            // 1. Dữ liệu 4 thẻ tài chính chính (Ảnh 8)
            ViewBag.DoanhThuThang = "452.8M";
            ViewBag.SoHoaDon = "1,245";
            ViewBag.NhapKhoVal = "128.5M";
            ViewBag.LoiNhuanVal = "145.2M";

            // 2. Dữ liệu biểu đồ tròn: Cơ cấu loại thuốc (%)
            ViewBag.CoCauThuoc = new List<dynamic>
            {
                new { Ten = "Thuốc kê đơn", TyLe = 45, Mau = "#0d6efd" },
                new { Ten = "Thực phẩm CN", TyLe = 30, Mau = "#6c757d" },
                new { Ten = "Dược mỹ phẩm", TyLe = 15, Mau = "#0dcaf0" },
                new { Ten = "Vật tư y tế", TyLe = 10, Mau = "#adb5bd" }
            };

            // 3. Nội dung Hardcode cho vùng AI Insights
            ViewBag.AiSuggestion = "Hệ thống phân tích nhu cầu dự báo Khẩu trang y tế và Siro ho Bảo Thanh sẽ tăng 30% trong 2 tuần tới do thời tiết chuyển mùa.";
            ViewBag.AiWarning = "Phát hiện 12 lô thuốc sắp hết hạn trong 30 ngày tới. Tổng giá trị lưu kho ước tính khoảng 4.5M đ. Cần có kế hoạch luân chuyển.";

            /* [NOTE LINQ CHO TƯƠNG LAI]:
               var thangNay = DateTime.Now.Month;
               ViewBag.DoanhThuThang = _context.HoaDons
                    .Where(h => h.NgayBan.Month == thangNay)
                    .Sum(h => h.TongTien).ToString("N0");
            */

            return View();
        }
    }
}