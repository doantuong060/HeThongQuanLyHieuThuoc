using Microsoft.AspNetCore.Mvc;
using DuAnCuoiKy_QuanLyHieuThuoc.Models;
using DuAnCuoiKy_QuanLyHieuThuoc.Business;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Controllers
{
    public class TongQuanKhoController : Controller
    {
        private readonly HieuThuocDbContext _context;
        private readonly TongQuanKhoBusiness _business;

        public TongQuanKhoController(HieuThuocDbContext context)
        {
            _context = context;
            _business = new TongQuanKhoBusiness(context);
        }

        public IActionResult Index()
        {
            // --- [LẤY DỮ LIỆU THẬT TỪ SQL THÔNG QUA TẦNG BUSINESS] ---

            // 1. Thống kê 3 thẻ trên cùng
            ViewBag.TongMatHang = _business.GetTongMatHang();
            ViewBag.SapHetHang = _business.GetSoLuongSapHetHang();
            ViewBag.SapHetHan = _business.GetSoLuongSapHetHan();

            // 2. Load các danh sách chi tiết (Lấy 5 dòng mới nhất để không vỡ UI)
            ViewBag.DsCanNhap = _business.GetDsCanNhap(5);
            ViewBag.DsHetHan = _business.GetDsHetHan(5);
            ViewBag.DsPhieuNhap = _business.GetDsPhieuNhap(5);

            return View();
        }
    }
}