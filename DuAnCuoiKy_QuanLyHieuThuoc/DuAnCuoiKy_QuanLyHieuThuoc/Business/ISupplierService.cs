using DuAnCuoiKy_QuanLyHieuThuoc.Models.ViewModels;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Business
{
    public interface ISupplierService
    {
        Task<SupplierIndexViewModel> GetIndexDataAsync(string search);
    }
}
