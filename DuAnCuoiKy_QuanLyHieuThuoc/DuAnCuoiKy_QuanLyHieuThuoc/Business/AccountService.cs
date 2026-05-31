using Microsoft.EntityFrameworkCore;
using DuAnCuoiKy_QuanLyHieuThuoc.Models;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Business
{
    public class AccountService : IAccountService
    {
        private readonly HieuThuocDbContext _context;
        public AccountService(HieuThuocDbContext context) => _context = context;

        public async Task<TaiKhoan?> AuthenticateAsync(string username, string password)
        {
            return await _context.TaiKhoans
                .Include(t => t.MaNvNavigation)
                .Include(t => t.MaVaiTroNavigation)
                .FirstOrDefaultAsync(u => u.TenDangNhap == username && u.MatKhau == password);
        }

        public async Task UpdateLastLoginAsync(string maNV)
        {
            var tk = await _context.TaiKhoans.FindAsync(maNV);
            if (tk != null)
            {
                tk.LanDangNhapCuoi = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }
    }
}