using Microsoft.AspNetCore.Mvc;
using DuAnCuoiKy_QuanLyHieuThuoc.Models;
using DuAnCuoiKy_QuanLyHieuThuoc.Business;
using System;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Controllers
{
    // ============================================
    // CONTROLLER XEM TỒN KHO (PHIÊN BẢN DATABASE THẬT)
    // ============================================
    public class KhoHangsController : Controller
    {
        private readonly HieuThuocDbContext _context;
        private readonly KhoHangBusiness _business;

        // Đổi lên 10 dòng/trang cho đúng chuẩn chuyên nghiệp như sếp yêu cầu lúc nãy
        private const int PAGE_SIZE = 10;

        public KhoHangsController(HieuThuocDbContext context)
        {
            _context = context;
            _business = new KhoHangBusiness(context);
        }

        public IActionResult Index(string keyword = "", string loai = "", int page = 1)
        {
            // Bắn dữ liệu xuống Business xử lý
            var result = _business.GetTonKhoThucTe(keyword, loai, page, PAGE_SIZE);

            // Xử lý toán học cho Phân trang
            int totalPages = (int)Math.Ceiling(result.TotalCount / (double)PAGE_SIZE);
            page = Math.Max(1, Math.Min(page, Math.Max(1, totalPages)));

            // Đẩy dữ liệu lên View
            ViewBag.Keyword = keyword;
            ViewBag.SelectedLoai = loai;
            ViewBag.Page = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalItems = result.TotalCount;
            ViewBag.PageSize = PAGE_SIZE;
            ViewBag.Categories = result.Categories;

            return View(result.Data);
        }
    }
}