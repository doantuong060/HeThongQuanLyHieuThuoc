using DuAnCuoiKy_QuanLyHieuThuoc.Models.ViewModels;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Business
{
    public interface IDashboardService
    {
        Task<AdminDashboardViewModel> GetAdminDashboardDataAsync();
    }
}