using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DuAnCuoiKy_QuanLyHieuThuoc.Business;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Controllers
{
    [Authorize(Roles = "QuanLy,DuocSi")] // Quản lý và Dược sĩ đều xem được
    public class SanPhamsController : Controller
    {
        private readonly IProductService _productService;
        public SanPhamsController(IProductService productService) => _productService = productService;

        public async Task<IActionResult> Index(string search, string loai)
        {
            var data = await _productService.GetProductIndexDataAsync(search, loai);
            ViewBag.CurrentSearch = search;
            ViewBag.CurrentLoai = loai;
            return View(data);
        }
    }
}