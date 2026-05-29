using DuAnCuoiKy_QuanLyHieuThuoc.Models.ViewModels;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Business
{
    public interface IProductService
    {
        Task<bool> AddProductAsync(AddProductViewModel model);
        Task<ProductIndexViewModel> GetProductIndexDataAsync(string search, string loai);
    }
}
