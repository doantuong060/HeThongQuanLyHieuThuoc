using Microsoft.AspNetCore.Mvc;
using DuAnCuoiKy_QuanLyHieuThuoc.Models;
using DuAnCuoiKy_QuanLyHieuThuoc.Business;
using DuAnCuoiKy_QuanLyHieuThuoc.Extensions;
using System;
using System.Linq;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Controllers
{
    public class TongQuanCaLamController : Controller
    {
        private readonly HieuThuocDbContext _context;
        private readonly TongQuanCaLamBusiness _business;

        public TongQuanCaLamController(HieuThuocDbContext context)
        {
            _context = context;
            _business = new TongQuanCaLamBusiness(context);
        }

        public IActionResult Index()
        {
            // Sử dụng Extension Method để lấy MaNV từ Claims, dự phòng "NV0002"
            string currentMaNV = User.Identity?.IsAuthenticated == true ? User.GetMaNV() : "NV0002";

            var nhanVien = _context.NhanViens.FirstOrDefault(nv => nv.MaNv == currentMaNV);
            ViewBag.TenNhanVien = nhanVien != null ? nhanVien.HoTen : "Chưa xác định";
            ViewBag.ChucVu = "Nhân viên bán hàng";
            ViewBag.NgayLamViec = DateTime.Now;
            ViewBag.TenCa = "Ca Sáng (07:00 – 15:00)";

            // Gọi tầng Business
            var thongKe = _business.GetThongKeTrongNgay(currentMaNV);

            ViewBag.SoHoaDonHomNay = thongKe.SoHoaDon;
            ViewBag.DoanhThuCaNay = thongKe.DoanhThu;
            ViewBag.SoKhachPhucVu = thongKe.SoKhach;

            ViewBag.ChartLabels = new string[] { "7h", "8h", "9h", "10h", "11h", "12h", "13h", "14h", "15h", "16h", "17h", "18h" };
            ViewBag.ChartData = thongKe.ChartData;

            var recentBills = _business.GetHoaDonGanNhat(currentMaNV);

            return View(recentBills);
        }
    }
}