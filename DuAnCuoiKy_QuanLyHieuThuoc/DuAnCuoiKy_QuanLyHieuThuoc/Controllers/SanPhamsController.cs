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
    [Authorize(Roles = "QuanLy,DuocSi")] // Cả 2 quyền đều có thể vào xem danh sách
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
        // 1. TRANG DANH SÁCH SẢN PHẨM (ẢNH 3)
        // ============================================================
        [HttpGet]
        public async Task<IActionResult> Index(string search, string loai)
        {
            // Lấy dữ liệu thống kê và danh sách từ SQL View thông qua Service
            var data = await _productService.GetProductIndexDataAsync(search, loai);

            // Gửi dữ liệu tìm kiếm hiện tại ra View để giữ trạng thái ô nhập
            ViewBag.CurrentSearch = search;
            ViewBag.CurrentLoai = loai;

            // Đổ dữ liệu vào các Dropdown trong Modal Thêm mới (Lấy từ SQL)
            ViewData["MaDvt"] = new SelectList(_context.DonViTinhs, "MaDvt", "TenDvt");
            ViewData["MaLoai"] = new SelectList(_context.LoaiThuocs, "MaLoai", "TenLoai");
            ViewData["MaLoaiVt"] = new SelectList(_context.LoaiVatTus, "MaLoaiVt", "TenLoaiVt");

            return View(data);
        }

        // ============================================================
        // 2. XỬ LÝ THÊM SẢN PHẨM MỚI (HÀNH ĐỘNG TỪ MODAL)
        // ============================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "QuanLy")] // Chỉ Quản lý mới được thêm sản phẩm
        public async Task<IActionResult> Create(AddProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Dữ liệu không hợp lệ. Vui lòng kiểm tra lại các trường bắt buộc.";
                return RedirectToAction(nameof(Index));
            }

            // GỌI BUSINESS LAYER: Xử lý lưu vào SanPham và bảng con (Thuoc/VatTu)
            bool result = await _productService.AddProductAsync(model);

            if (result)
            {
                TempData["Success"] = $"Thêm sản phẩm '{model.TenSp}' thành công!";
            }
            else
            {
                TempData["Error"] = "Lỗi hệ thống: Không thể lưu sản phẩm vào Database.";
            }

            return RedirectToAction(nameof(Index));
        }

        // ============================================================
        // 3. THAY ĐỔI TRẠNG THÁI KINH DOANH (NGỪNG BÁN/ĐANG BÁN)
        // ============================================================
        [HttpPost]
        [Authorize(Roles = "QuanLy")]
        public async Task<IActionResult> ToggleStatus(string id)
        {
            var sp = await _context.SanPhams.FindAsync(id);
            if (sp == null) return NotFound();

            sp.TrangThai = !sp.TrangThai; // Đảo ngược trạng thái bit
            await _context.SaveChangesAsync();

            TempData["Success"] = "Đã cập nhật trạng thái kinh doanh của sản phẩm.";
            return RedirectToAction(nameof(Index));
        }

        // ============================================================
        // 4. XÓA SẢN PHẨM (KIỂM TRA RÀNG BUỘC SQL)
        // ============================================================
        [HttpPost]
        [Authorize(Roles = "QuanLy")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var sp = await _context.SanPhams.FindAsync(id);
                if (sp == null) return NotFound();

                // Kiểm tra xem sản phẩm đã có trong Lô hàng hay Hóa đơn chưa
                bool hasHistory = await _context.LoHangs.AnyAsync(l => l.MaSp == id) ||
                                  await _context.ChiTietHoaDons.AnyAsync(c => c.SoLoNavigation.MaSp == id);

                if (hasHistory)
                {
                    TempData["Error"] = "Không thể xóa sản phẩm đã có lịch sử nhập xuất. Hãy sử dụng chức năng 'Ngừng kinh doanh'.";
                    return RedirectToAction(nameof(Index));
                }

                // Xóa bảng con trước (Do ràng buộc khóa ngoại trong SQL)
                var thuoc = await _context.Thuocs.FindAsync(id);
                if (thuoc != null) _context.Thuocs.Remove(thuoc);

                var vtyt = await _context.VatTuYtes.FindAsync(id);
                if (vtyt != null) _context.VatTuYtes.Remove(vtyt);

                _context.SanPhams.Remove(sp);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Đã xóa sản phẩm khỏi hệ thống.";
            }
            catch (Exception)
            {
                TempData["Error"] = "Lỗi xung đột dữ liệu SQL. Không thể xóa.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}