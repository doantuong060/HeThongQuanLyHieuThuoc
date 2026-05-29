using DuAnCuoiKy_QuanLyHieuThuoc.Business;
using DuAnCuoiKy_QuanLyHieuThuoc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Controllers
{
    [Authorize(Roles = "QuanLy")] // Chỉ quản lý mới được xem đối tác
    public class NhaCungCapsController : Controller
    {
        private readonly ISupplierService _supplierService;
        private readonly HieuThuocDbContext _context;

        public NhaCungCapsController(ISupplierService supplierService, HieuThuocDbContext context)
        {
            _supplierService = supplierService;
            _context = context;
        }

        public async Task<IActionResult> Index(string search)
        {
            var data = await _supplierService.GetIndexDataAsync(search);
            ViewBag.CurrentSearch = search;

            // Load danh sách Tỉnh thành cho Modal Thêm mới
            ViewData["MaTinhThanh"] = new SelectList(_context.TinhThanhs, "MaTinhThanh", "TenTinhThanh");

            return View(data);
        }

    }
}