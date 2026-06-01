using DuAnCuoiKy_QuanLyHieuThuoc.Business;
using DuAnCuoiKy_QuanLyHieuThuoc.Models;
using DuAnCuoiKy_QuanLyHieuThuoc.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Controllers
{
    [Authorize(Roles = "QuanLy")]
    public class NhaCungCapsController : Controller
    {
        private readonly ISupplierService _supplierService;
        private readonly HieuThuocDbContext _context;

        public NhaCungCapsController(ISupplierService supplierService, HieuThuocDbContext context)
        {
            _supplierService = supplierService;
            _context = context;
        }

        // ============================================================
        // 1. TRANG DANH SÁCH
        // ============================================================
        public async Task<IActionResult> Index(string search)
        {
            var data = await _supplierService.GetIndexDataAsync(search);
            ViewBag.CurrentSearch = search;
            ViewData["MaTinhThanh"] = new SelectList(_context.TinhThanhs, "MaTinhThanh", "TenTinhThanh");
            return View(data);
        }

        // ============================================================
        // 2. LẤY CHI TIẾT NCC (AJAX - trả JSON cho modal xem/sửa)
        // ============================================================
        [HttpGet]
        public async Task<IActionResult> GetDetail(string id)
        {
            if (string.IsNullOrEmpty(id))
                return Json(new { success = false, message = "Mã không hợp lệ." });

            var ncc = await _context.NhaCungCaps
                .Include(n => n.MaPhuongXaNavigation)
                    .ThenInclude(p => p.MaTinhThanhNavigation)
                .FirstOrDefaultAsync(n => n.MaNcc == id);

            if (ncc == null)
                return Json(new { success = false, message = "Không tìm thấy nhà cung cấp." });

            return Json(new
            {
                success = true,
                maNcc = ncc.MaNcc,
                tenNcc = ncc.TenNcc,
                soDienThoai = ncc.SoDienThoai,
                email = ncc.Email,
                diaChi = ncc.DiaChi,
                trangThai = ncc.TrangThai,
                tenTinhThanh = ncc.MaPhuongXaNavigation?.MaTinhThanhNavigation?.TenTinhThanh ?? "Chưa rõ"
            });
        }

        // ============================================================
        // 3. THÊM MỚI
        // ============================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddSupplierViewModel model)
        {
            if (!ModelState.IsValid)
            {
                string loiValidation = string.Join("; ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                TempData["Error"] = "Dữ liệu không hợp lệ: " + loiValidation;
                return RedirectToAction(nameof(Index));
            }

            bool ketQua = await _supplierService.AddSupplierAsync(model);

            if (ketQua)
                TempData["Success"] = $"Thêm nhà cung cấp '{model.TenNcc}' thành công!";
            else
                TempData["Error"] = "Lỗi hệ thống: Không thể lưu nhà cung cấp vào Database.";

            return RedirectToAction(nameof(Index));
        }

        // ============================================================
        // 4. CẬP NHẬT
        // ============================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditSupplierViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Dữ liệu không hợp lệ. Vui lòng kiểm tra lại.";
                return RedirectToAction(nameof(Index));
            }

            bool ketQua = await _supplierService.UpdateSupplierAsync(model);

            if (ketQua)
                TempData["Success"] = $"Cập nhật nhà cung cấp '{model.TenNcc}' thành công!";
            else
                TempData["Error"] = "Không tìm thấy nhà cung cấp hoặc lỗi hệ thống.";

            return RedirectToAction(nameof(Index));
        }

        // ============================================================
        // 5. XÓA
        // ============================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            bool ketQua = await _supplierService.DeleteSupplierAsync(id);

            if (ketQua)
                TempData["Success"] = "Đã xóa nhà cung cấp khỏi hệ thống.";
            else
                TempData["Error"] = "Không thể xóa: Nhà cung cấp đã có phiếu nhập liên quan hoặc không tồn tại.";

            return RedirectToAction(nameof(Index));
        }
    }
}