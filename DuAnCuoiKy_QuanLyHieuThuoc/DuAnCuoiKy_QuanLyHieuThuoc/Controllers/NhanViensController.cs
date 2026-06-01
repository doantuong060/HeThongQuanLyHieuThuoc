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
    [Authorize(Roles = "QuanLy")]
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
        // 1. TRANG DANH SÁCH
        // ============================================================
        [HttpGet]
        public async Task<IActionResult> Index(string search)
        {
            var data = await _staffService.GetStaffIndexDataAsync(search);
            ViewBag.CurrentSearch = search;
            ViewData["MaVaiTro"] = new SelectList(_context.VaiTros, "MaVaiTro", "TenVaiTro");
            return View(data);
        }

        // ============================================================
        // 2. THÊM NHÂN VIÊN MỚI
        // ============================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddStaffViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);
                TempData["Error"] = string.Join(" | ", errors);
                return RedirectToAction(nameof(Index));
            }

            bool isUsernameTaken = await _context.TaiKhoans
                .AnyAsync(t => t.TenDangNhap == model.TenDangNhap);
            if (isUsernameTaken)
            {
                TempData["Error"] = "Tên đăng nhập này đã được sử dụng.";
                return RedirectToAction(nameof(Index));
            }

            bool isPhoneTaken = await _context.NhanViens
                .AnyAsync(n => n.SoDienThoai == model.SoDienThoai);
            if (isPhoneTaken)
            {
                TempData["Error"] = "Số điện thoại này đã tồn tại trên hệ thống.";
                return RedirectToAction(nameof(Index));
            }

            bool result = await _staffService.AddStaffAsync(model);
            TempData[result ? "Success" : "Error"] = result
                ? $"Thêm nhân viên {model.HoTen} thành công!"
                : "Lỗi lưu Database. Kiểm tra mật khẩu phải có CHỮ HOA + chữ thường + ký tự @.";

            return RedirectToAction(nameof(Index));
        }

        // ============================================================
        // 3. LẤY DỮ LIỆU NHÂN VIÊN CHO MODAL SỬA (AJAX GET)
        // ============================================================
        [HttpGet]
        public async Task<IActionResult> GetForEdit(string id)
        {
            var model = await _staffService.GetStaffForEditAsync(id);
            if (model == null) return NotFound();
            return Json(model);
        }

        // ============================================================
        // 4. LƯU CHỈNH SỬA NHÂN VIÊN
        // ============================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditStaffViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);
                TempData["Error"] = string.Join(" | ", errors);
                return RedirectToAction(nameof(Index));
            }

            // Kiểm tra trùng SĐT (trừ chính nhân viên đang sửa)
            bool isPhoneTaken = await _context.NhanViens
                .AnyAsync(n => n.SoDienThoai == model.SoDienThoai && n.MaNv != model.MaNv);
            if (isPhoneTaken)
            {
                TempData["Error"] = "Số điện thoại này đã được dùng bởi nhân viên khác.";
                return RedirectToAction(nameof(Index));
            }

            bool result = await _staffService.UpdateStaffAsync(model);
            TempData[result ? "Success" : "Error"] = result
                ? $"Cập nhật nhân viên {model.HoTen} thành công!"
                : "Lỗi lưu Database. Kiểm tra mật khẩu mới phải có CHỮ HOA + chữ thường + ký tự @.";

            return RedirectToAction(nameof(Index));
        }

        // ============================================================
        // 5. KHÓA / MỞ KHÓA
        // ============================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(string id)
        {
            var nv = await _context.NhanViens.FindAsync(id);
            if (nv != null)
            {
                nv.TrangThai = !nv.TrangThai;
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Đã {(nv.TrangThai ? "mở khóa" : "khóa")} tài khoản nhân viên thành công.";
            }
            else
            {
                TempData["Error"] = "Không tìm thấy nhân viên.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}