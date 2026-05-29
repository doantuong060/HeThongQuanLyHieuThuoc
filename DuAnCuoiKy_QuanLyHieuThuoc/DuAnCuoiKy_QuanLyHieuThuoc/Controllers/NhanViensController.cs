using DuAnCuoiKy_QuanLyHieuThuoc.Business;
using DuAnCuoiKy_QuanLyHieuThuoc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Controllers
{
    [Authorize(Roles = "QuanLy")] // Bảo mật cấp độ Admin
    public class NhanViensController : Controller
    {
        private readonly IStaffService _staffService;
        private readonly HieuThuocDbContext _context;

        public NhanViensController(IStaffService staffService, HieuThuocDbContext context)
        {
            _staffService = staffService;
            _context = context;
        }

        public async Task<IActionResult> Index(string search)
        {
            var data = await _staffService.GetStaffIndexDataAsync(search);
            ViewBag.CurrentSearch = search;

            // Lấy danh sách Vai trò cho Modal thêm mới
            ViewBag.MaVaiTro = new SelectList(_context.VaiTros, "MaVaiTro", "TenVaiTro");

            return View(data);
        }
    }
}