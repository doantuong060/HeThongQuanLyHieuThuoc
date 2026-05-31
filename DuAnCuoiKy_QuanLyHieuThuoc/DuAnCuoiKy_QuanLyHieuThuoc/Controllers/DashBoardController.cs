using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DuAnCuoiKy_QuanLyHieuThuoc.Business;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Controllers
{
    [Authorize(Roles = "QuanLy")]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;
        public DashboardController(IDashboardService dashboardService) => _dashboardService = dashboardService;

        public async Task<IActionResult> Index_Admin()
        {
            var data = await _dashboardService.GetAdminDashboardDataAsync();
            return View(data);
        }
    }
}