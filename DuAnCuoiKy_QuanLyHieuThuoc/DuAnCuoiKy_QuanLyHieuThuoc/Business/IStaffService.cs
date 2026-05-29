using DuAnCuoiKy_QuanLyHieuThuoc.Models.ViewModels;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Business
{
    public interface IStaffService
    {
        Task<StaffIndexViewModel> GetStaffIndexDataAsync(string search);
    }
}
