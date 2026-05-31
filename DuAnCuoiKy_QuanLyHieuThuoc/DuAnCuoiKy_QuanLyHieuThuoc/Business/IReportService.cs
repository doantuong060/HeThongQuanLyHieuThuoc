using DuAnCuoiKy_QuanLyHieuThuoc.Models.ViewModels;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Business
{
    public interface IReportService
    {
        Task<ReportViewModel> GetFullReportAsync();
    }
}
