using Microsoft.AspNetCore.Mvc;
using DuAnCuoiKy_QuanLyHieuThuoc.Models;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Controllers
{
    // ============================================
    // CONTROLLER TỔNG QUAN CA LÀM
    // ============================================
    public class TongQuanCaLamController : Controller
    {
        // ============================================
        // TRANG TỔNG QUAN CA LÀM
        // ============================================
        public IActionResult Index()
        {
            // ── THÔNG TIN CA LÀM (FAKE) ──────────────────
            ViewBag.TenNhanVien = "Trần Văn Hùng";
            ViewBag.ChucVu = "Nhân viên bán hàng";
            ViewBag.NgayLamViec = DateTime.Now; // Hiển thị ngày hiện tại
            ViewBag.TenCa = "Ca Sáng (07:00 – 15:00)";

            // ── 3 THẺ THỐNG KÊ (FAKE) ─────────────────────
            ViewBag.SoHoaDonHomNay = 12;
            ViewBag.DoanhThuCaNay = 1_250_000m;
            ViewBag.SoKhachPhucVu = 12;

            // ── 5 HÓA ĐƠN GẦN NHẤT (FAKE) ────────────────
            var hoaDons = new List<HoaDonCaLam>
            {
                new HoaDonCaLam { MaHD = "HD-1024", ThoiGian = "09:45 AM", TongTien = 125_000, TrangThai = "Hoàn thành"      },
                new HoaDonCaLam { MaHD = "HD-1023", ThoiGian = "09:12 AM", TongTien = 450_000, TrangThai = "Hoàn thành"      },
                new HoaDonCaLam { MaHD = "HD-1022", ThoiGian = "08:55 AM", TongTien =  85_000, TrangThai = "Chờ thanh toán"  },
                new HoaDonCaLam { MaHD = "HD-1021", ThoiGian = "08:30 AM", TongTien = 320_000, TrangThai = "Hoàn thành"      },
                new HoaDonCaLam { MaHD = "HD-1020", ThoiGian = "07:45 AM", TongTien = 270_000, TrangThai = "Hoàn thành"      },
            };

            // ── DỮ LIỆU BIỂU ĐỒ THEO GIỜ (FAKE) ──────────
            // Mảng 12 giá trị = số hóa đơn theo từng giờ: 7h → 18h
            ViewBag.ChartLabels = new[] { "7h", "8h", "9h", "10h", "11h", "12h", "13h", "14h", "15h", "16h", "17h", "18h" };
            ViewBag.ChartData = new[] { 0, 3, 8, 5, 0, 0, 0, 0, 0, 0, 0, 0 };

            return View(hoaDons);
        }
    }
}