using DuAnCuoiKy_QuanLyHieuThuoc.Models.ViewModels;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Business
{
    public interface IStaffService
    {
        Task<bool> AddStaffAsync(AddStaffViewModel model);
        Task<StaffIndexViewModel> GetStaffIndexDataAsync(string search);
    }
}
