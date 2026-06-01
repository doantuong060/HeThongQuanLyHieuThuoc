using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DuAnCuoiKy_QuanLyHieuThuoc.Models;
using DuAnCuoiKy_QuanLyHieuThuoc.Business;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Controllers
{
    public class HoaDonsController : Controller
    {
        private readonly HieuThuocDbContext _context;
        private readonly LichSuHoaDonBusiness _business;

        public HoaDonsController(HieuThuocDbContext context)
        {
            _context = context;
            _business = new LichSuHoaDonBusiness(context);
        }

        // ==========================================
        // 1. HIỂN THỊ DANH SÁCH HÓA ĐƠN (Có bộ lọc)
        // ==========================================
        // GET: HoaDons
        // Khai báo thêm tham số page (mặc định là trang 1 nếu không truyền)
        public IActionResult Index(string searchKeyword, string timeFilter, string statusFilter, int page = 1)
        {
            int pageSize = 10; // Cài đặt hiển thị đúng 10 dòng/trang theo ý sếp

            // Lấy kết quả từ Business
            var result = _business.GetLichSuHoaDon(searchKeyword, timeFilter, statusFilter, page, pageSize);

            // Gửi dữ liệu lọc về View
            ViewBag.SearchKeyword = searchKeyword;
            ViewBag.TimeFilter = timeFilter;
            ViewBag.StatusFilter = statusFilter;

            // Gửi các thông số phân trang về View để vẽ nút bấm
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalCount = result.TotalCount;
            ViewBag.TotalPages = (int)Math.Ceiling(result.TotalCount / (double)pageSize);

            // Truyền riêng danh sách 10 hóa đơn vào Model
            return View(result.Data);
        }

        // ==========================================
        // 2. XEM CHI TIẾT HÓA ĐƠN (In biên lai)
        // ==========================================
        // GET: HoaDons/Details/HD0001
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            // Dùng Eager Loading để móc nối dữ liệu: Hóa Đơn -> Chi Tiết HĐ -> Lô Hàng -> Sản Phẩm
            var hoaDon = await _context.HoaDons
                .Include(h => h.MaNvNavigation)       // Lấy tên nhân viên
                .Include(h => h.MaKhNavigation)       // Lấy tên khách hàng (nếu có)
                .Include(h => h.ChiTietHoaDons)       // Lấy danh sách thuốc đã mua
                    .ThenInclude(ct => ct.SoLoNavigation)
                        .ThenInclude(sl => sl.MaSpNavigation)
                .FirstOrDefaultAsync(m => m.MaHd == id);

            if (hoaDon == null)
            {
                return NotFound();
            }

            return View(hoaDon);
        }
    }
}