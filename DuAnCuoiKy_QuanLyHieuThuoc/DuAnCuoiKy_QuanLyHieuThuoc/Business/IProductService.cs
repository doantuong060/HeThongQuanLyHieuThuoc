using DuAnCuoiKy_QuanLyHieuThuoc.Models.ViewModels;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Business
{
    public interface IProductService
    {
        Task<ProductIndexViewModel> GetProductIndexDataAsync(string search, string loai);
    }
}
