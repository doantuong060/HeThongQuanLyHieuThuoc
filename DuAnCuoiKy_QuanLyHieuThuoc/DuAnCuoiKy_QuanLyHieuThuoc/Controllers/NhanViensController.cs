using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DuAnCuoiKy_QuanLyHieuThuoc.Models;
using DuAnCuoiKy_QuanLyHieuThuoc.Models.ViewModels;
using DuAnCuoiKy_QuanLyHieuThuoc.Business;
using DuAnCuoiKy_QuanLyHieuThuoc.Enums;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Controllers
{
    [Authorize(Roles = "QuanLy")] // Chỉ Quản lý mới có quyền vào phân hệ này
    public class NhanViensController : Controller
    {
        private readonly IStaffService _staffService;
        private readonly HieuThuocDbContext _context;

        public NhanViensController(IStaffService staffService, HieuThuocDbContext context)
        {
            _staffService = staffService;
            _context = context;
        }

        // ============================================================
        // 1. TRANG DANH SÁCH NHÂN VIÊN (ẢNH 4)
        // ============================================================
        [HttpGet]
        public async Task<IActionResult> Index(string search)
        {
            // Lấy dữ liệu từ Business Layer (đã bao gồm các thống kê SQL)
            var data = await _staffService.GetStaffIndexDataAsync(search);

            // Lưu lại từ khóa tìm kiếm để hiển thị lại trên ô Input
            ViewBag.CurrentSearch = search;

            // Lấy danh sách Vai trò từ SQL để đổ vào Dropdown trong Modal Thêm mới
            // Dựa đúng vào bảng VaiTro trong file SQL của bạn
            ViewData["MaVaiTro"] = new SelectList(_context.VaiTros, "MaVaiTro", "TenVaiTro");

            return View(data);
        }

        // ============================================================
        // 2. XỬ LÝ THÊM NHÂN VIÊN MỚI (ẢNH 5 - TỪ MODAL)
        // ============================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddStaffViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Dữ liệu nhập vào không hợp lệ. Vui lòng kiểm tra lại.";
                return RedirectToAction(nameof(Index));
            }

            // [BUSINESS LOGIC]: Kiểm tra trùng tên đăng nhập trong bảng TaiKhoan
            bool isUsernameTaken = await _context.TaiKhoans
                .AnyAsync(t => t.TenDangNhap == model.TenDangNhap);

            if (isUsernameTaken)
            {
                TempData["Error"] = "Tên đăng nhập này đã được sử dụng. Vui lòng chọn tên khác.";
                return RedirectToAction(nameof(Index));
            }

            // [BUSINESS LOGIC]: Kiểm tra trùng Số điện thoại trong bảng NhanVien
            bool isPhoneTaken = await _context.NhanViens
                .AnyAsync(n => n.SoDienThoai == model.SoDienThoai);

            if (isPhoneTaken)
            {
                TempData["Error"] = "Số điện thoại này đã tồn tại trên hệ thống.";
                return RedirectToAction(nameof(Index));
            }

            // GỌI SERVICE ĐỂ THỰC HIỆN TRANSACTION LƯU VÀO 2 BẢNG SQL
            bool result = await _staffService.AddStaffAsync(model);

            if (result)
            {
                TempData["Success"] = $"Thêm nhân viên {model.HoTen} thành công!";
            }
            else
            {
                TempData["Error"] = "Đã xảy ra lỗi trong quá trình lưu vào Database.";
            }

            return RedirectToAction(nameof(Index));
        }

        // ============================================================
        // 3. XỬ LÝ KHÓA/MỞ KHÓA NHÂN VIÊN
        // ============================================================
        [HttpPost]
        public async Task<IActionResult> ToggleStatus(string id)
        {
            var nv = await _context.NhanViens.FindAsync(id);
            if (nv != null)
            {
                // Đảo ngược trạng thái (True <-> False)
                nv.TrangThai = !nv.TrangThai;
                await _context.SaveChangesAsync();
                TempData["Success"] = "Cập nhật trạng thái nhân viên thành công.";
            }
            return RedirectToAction(nameof(Index));
        }

        // ============================================================
        // 4. XEM CHI TIẾT NHÂN VIÊN (DÀNH CHO AJAX NẾU CẦN)
        // ============================================================
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var nv = await _context.NhanViens
                .Include(n => n.TaiKhoan)
                .ThenInclude(t => t.MaVaiTroNavigation)
                .FirstOrDefaultAsync(m => m.MaNv == id);

            if (nv == null) return NotFound();
            return Json(nv);
        }
    }
}