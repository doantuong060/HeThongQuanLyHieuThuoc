using DuAnCuoiKy_QuanLyHieuThuoc.Models;
using DuAnCuoiKy_QuanLyHieuThuoc.Models.ViewModels;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Business
{
    public interface IAccountService
    {
        Task<TaiKhoan?> AuthenticateAsync(string username, string password);
        Task UpdateLastLoginAsync(string maNV);
    }
}