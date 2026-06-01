using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DuAnCuoiKy_QuanLyHieuThuoc.Models;
using DuAnCuoiKy_QuanLyHieuThuoc.Models.ViewModels;
using DuAnCuoiKy_QuanLyHieuThuoc.Business;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Controllers
{
    [Authorize(Roles = "QuanLy,DuocSi")]
    public class SanPhamsController : Controller
    {
        private readonly IProductService _productService;
        private readonly HieuThuocDbContext _context;

        public SanPhamsController(IProductService productService, HieuThuocDbContext context)
        {
            _productService = productService;
            _context = context;
        }

        // ============================================================
        // 1. TRANG DANH SÁCH SẢN PHẨM
        // ============================================================
        [HttpGet]
        public async Task<IActionResult> Index(string search, string loai)
        {
            var data = await _productService.GetProductIndexDataAsync(search, loai);

            ViewBag.CurrentSearch = search;
            ViewBag.CurrentLoai = loai;

            // Dropdown cho Modal Thêm mới
            ViewData["MaDvt"] = new SelectList(_context.DonViTinhs, "MaDvt", "TenDvt");
            ViewData["MaLoai"] = new SelectList(_context.LoaiThuocs, "MaLoai", "TenLoai");
            ViewData["MaLoaiVt"] = new SelectList(_context.LoaiVatTus, "MaLoaiVt", "TenLoaiVt");

            return View(data);
        }

        // ============================================================
        // 2. THÊM SẢN PHẨM MỚI (POST từ Modal)
        // ============================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "QuanLy")]
        public async Task<IActionResult> Create(AddProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                string loiValidation = string.Join("; ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                TempData["Error"] = "Dữ liệu không hợp lệ: " + loiValidation;
                return RedirectToAction(nameof(Index));
            }

            bool result = await _productService.AddProductAsync(model);

            if (result)
                TempData["Success"] = $"Thêm sản phẩm '{model.TenSp}' thành công!";
            else
                TempData["Error"] = "Lỗi hệ thống: Không thể lưu sản phẩm vào Database.";

            return RedirectToAction(nameof(Index));
        }

        // ============================================================
        // 3. LẤY THÔNG TIN SẢN PHẨM ĐỂ SỬA (AJAX GET - trả về JSON)
        // ============================================================
        [HttpGet]
        [Authorize(Roles = "QuanLy")]
        public async Task<IActionResult> GetDetail(string id)
        {
            if (string.IsNullOrEmpty(id))
                return Json(new { success = false, message = "Mã sản phẩm không hợp lệ." });

            var sp = await _context.SanPhams
                .Include(s => s.Thuoc)
                .Include(s => s.VatTuYte)
                .Include(s => s.MaDvtNavigation)
                .FirstOrDefaultAsync(s => s.MaSp == id);

            if (sp == null)
                return Json(new { success = false, message = "Không tìm thấy sản phẩm." });

            // Trả về object phẳng để JS đổ vào form
            var result = new
            {
                success = true,
                maSp = sp.MaSp,
                tenSp = sp.TenSp,
                maDvt = sp.MaDvt,
                giaBan = sp.GiaBan,
                mucCanhBao = sp.MucCanhBao,
                loaiSp = sp.LoaiSp,
                trangThai = sp.TrangThai,
                // Thuốc
                maLoai = sp.Thuoc?.MaLoai,
                canToa = sp.Thuoc?.CanToa ?? false,
                ghiChu = sp.Thuoc?.GhiChu,
                // Vật tư
                maLoaiVt = sp.VatTuYte?.MaLoaiVt,
                nhaSanXuat = sp.VatTuYte?.NhaSanXuat
            };

            return Json(result);
        }

        // ============================================================
        // 4. CẬP NHẬT SẢN PHẨM (POST từ Modal Sửa)
        // ============================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "QuanLy")]
        public async Task<IActionResult> Edit(EditProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Dữ liệu không hợp lệ. Vui lòng kiểm tra lại.";
                return RedirectToAction(nameof(Index));
            }

            var sp = await _context.SanPhams
                .Include(s => s.Thuoc)
                .Include(s => s.VatTuYte)
                .FirstOrDefaultAsync(s => s.MaSp == model.MaSp);

            if (sp == null)
            {
                TempData["Error"] = "Không tìm thấy sản phẩm cần sửa.";
                return RedirectToAction(nameof(Index));
            }

            // Cập nhật bảng SanPham
            sp.TenSp = model.TenSp;
            sp.MaDvt = model.MaDvt;
            sp.GiaBan = model.GiaBan;
            sp.MucCanhBao = model.MucCanhBao;

            // Cập nhật bảng con theo LoaiSp
            if (sp.LoaiSp == "THUOC" && sp.Thuoc != null)
            {
                sp.Thuoc.MaLoai = model.MaLoai ?? sp.Thuoc.MaLoai;
                sp.Thuoc.CanToa = model.CanToa;
                sp.Thuoc.GhiChu = model.GhiChu;
            }
            else if (sp.LoaiSp == "VATTU" && sp.VatTuYte != null)
            {
                sp.VatTuYte.MaLoaiVt = model.MaLoaiVt ?? sp.VatTuYte.MaLoaiVt;
                sp.VatTuYte.NhaSanXuat = model.NhaSanXuat;
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = $"Cập nhật sản phẩm '{sp.TenSp}' thành công!";

            return RedirectToAction(nameof(Index));
        }

        // ============================================================
        // 5. THAY ĐỔI TRẠNG THÁI KINH DOANH
        // ============================================================
        [HttpPost]
        [Authorize(Roles = "QuanLy")]
        public async Task<IActionResult> ToggleStatus(string id)
        {
            var sp = await _context.SanPhams.FindAsync(id);
            if (sp == null) return NotFound();

            sp.TrangThai = !sp.TrangThai;
            await _context.SaveChangesAsync();

            TempData["Success"] = "Đã cập nhật trạng thái kinh doanh của sản phẩm.";
            return RedirectToAction(nameof(Index));
        }

        // ============================================================
        // 6. XÓA SẢN PHẨM
        // ============================================================
        [HttpPost]
        [Authorize(Roles = "QuanLy")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var sp = await _context.SanPhams.FindAsync(id);
                if (sp == null)
                {
                    TempData["Error"] = "Không tìm thấy sản phẩm.";
                    return RedirectToAction(nameof(Index));
                }

                // Kiểm tra ràng buộc: đã có lô hàng hoặc chi tiết hóa đơn
                bool coLoHang = await _context.LoHangs.AnyAsync(l => l.MaSp == id);
                bool coHoaDon = await _context.ChiTietHoaDons
                    .AnyAsync(c => c.SoLoNavigation.MaSp == id);

                if (coLoHang || coHoaDon)
                {
                    TempData["Error"] = $"Không thể xóa '{sp.TenSp}' vì đã có lịch sử nhập/xuất. Hãy dùng chức năng 'Ngừng kinh doanh'.";
                    return RedirectToAction(nameof(Index));
                }

                // Xóa bảng con trước (FK constraint)
                var thuoc = await _context.Thuocs.FindAsync(id);
                if (thuoc != null) _context.Thuocs.Remove(thuoc);

                var vtyt = await _context.VatTuYtes.FindAsync(id);
                if (vtyt != null) _context.VatTuYtes.Remove(vtyt);

                _context.SanPhams.Remove(sp);
                await _context.SaveChangesAsync();

                TempData["Success"] = $"Đã xóa sản phẩm '{sp.TenSp}' khỏi hệ thống.";
            }
            catch (Exception)
            {
                TempData["Error"] = "Lỗi xung đột dữ liệu SQL. Không thể xóa sản phẩm này.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}