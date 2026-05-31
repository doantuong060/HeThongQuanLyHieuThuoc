using DuAnCuoiKy_QuanLyHieuThuoc.Business;
using DuAnCuoiKy_QuanLyHieuThuoc.Extensions;
using DuAnCuoiKy_QuanLyHieuThuoc.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Controllers
{
    public class TongQuanCaLamController : Controller
    {
        private readonly TongQuanCaLamBusiness _business;
        public TongQuanCaLamController(HieuThuocDbContext context) => _business = new TongQuanCaLamBusiness(context);

        public IActionResult Index()
        {
            string maNV = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "NV0002";
            var thongKe = _business.GetThongKeTrongNgay(maNV);
            ViewBag.SoHoaDonHomNay = thongKe.SoHoaDon;
            ViewBag.DoanhThuCaNay = thongKe.DoanhThu;
            ViewBag.SoKhachPhucVu = thongKe.SoKhach;
            ViewBag.ChartLabels = new string[] { "7h", "8h", "9h", "10h", "11h", "12h", "13h", "14h", "15h", "16h", "17h", "18h" };
            ViewBag.ChartData = thongKe.ChartData;
            ViewBag.NgayLamViec = DateTime.Now;
            ViewBag.TenCa = "Ca Sáng (07:00 – 15:00)";
            return View(_business.GetHoaDonGanNhat(maNV));
        }

        [HttpGet]
        public IActionResult GetTatCaHoaDon()
        {
            string maNV = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "NV0002";
            return Json(new { success = true, data = _business.GetAllHoaDonTrongNgay(maNV) });
        }

        public IActionResult GetChiTiet(string maHD)
        {
            return Json(new { success = true, data = _business.GetChiTietHoaDon(maHD) });
        }
    }
}