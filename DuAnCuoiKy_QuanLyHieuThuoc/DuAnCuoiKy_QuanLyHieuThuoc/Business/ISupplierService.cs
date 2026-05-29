using DuAnCuoiKy_QuanLyHieuThuoc.Models.ViewModels;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Business
{
    public interface ISupplierService
    {
        Task<bool> AddSupplierAsync(AddSupplierViewModel model);
        Task<SupplierIndexViewModel> GetIndexDataAsync(string search);
    }
}
