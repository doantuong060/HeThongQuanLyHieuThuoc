using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DuAnCuoiKy_QuanLyHieuThuoc.Business;
using DuAnCuoiKy_QuanLyHieuThuoc.Enums;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Controllers
{
    [Authorize(Roles = "QuanLy")] // Bảo mật: Chỉ Quản lý mới được vào
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;
        public DashboardController(IDashboardService dashboardService) => _dashboardService = dashboardService;

        public async Task<IActionResult> Index_Admin()
        {
            // Gọi Business Service để lấy dữ liệu thực từ SQL
            var data = await _dashboardService.GetAdminDashboardDataAsync();
            return View(data);
        }
    }
}