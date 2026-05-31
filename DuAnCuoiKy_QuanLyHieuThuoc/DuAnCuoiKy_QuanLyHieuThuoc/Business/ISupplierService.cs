using DuAnCuoiKy_QuanLyHieuThuoc.Models.ViewModels;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Business
{
    public interface ISupplierService
    {
        Task<SupplierIndexViewModel> GetIndexDataAsync(string search);
        Task<bool> AddSupplierAsync(AddSupplierViewModel model);
        Task<bool> UpdateSupplierAsync(EditSupplierViewModel model);
        Task<bool> DeleteSupplierAsync(string maNcc);
    }
}