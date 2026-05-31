using DuAnCuoiKy_QuanLyHieuThuoc.Models;
using DuAnCuoiKy_QuanLyHieuThuoc.Models.ViewModels;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Business
{
    public interface IAccountService
    {
        // Kiểm tra đăng nhập và trả về thông tin tài khoản kèm Role
        Task<TaiKhoan?> AuthenticateAsync(string username, string password);

        // Cập nhật thời gian đăng nhập cuối
        Task UpdateLastLoginAsync(string maNV);
    }
}