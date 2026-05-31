using DuAnCuoiKy_QuanLyHieuThuoc.Models.ViewModels;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Business
{
    public interface IStaffService
    {
        Task<StaffIndexViewModel> GetStaffIndexDataAsync(string search);
        Task<bool> AddStaffAsync(AddStaffViewModel model);
        Task<EditStaffViewModel?> GetStaffForEditAsync(string maNv);
        Task<bool> UpdateStaffAsync(EditStaffViewModel model);
    }
}