using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DuAnCuoiKy_QuanLyHieuThuoc.Business;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Controllers
{
    [Authorize(Roles = "QuanLy")]
    public class BaoCaoController : Controller
    {
        private readonly IReportService _reportService;
        public BaoCaoController(IReportService reportService) => _reportService = reportService;

        public async Task<IActionResult> Index()
        {
            var data = await _reportService.GetFullReportAsync();
            return View(data);
        }
    }
}