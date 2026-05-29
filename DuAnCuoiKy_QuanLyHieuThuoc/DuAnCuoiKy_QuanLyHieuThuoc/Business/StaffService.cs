using Microsoft.EntityFrameworkCore;
using DuAnCuoiKy_QuanLyHieuThuoc.Models;
using DuAnCuoiKy_QuanLyHieuThuoc.Models.ViewModels;
using DuAnCuoiKy_QuanLyHieuThuoc.Enums;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Business
{
    public class StaffService : IStaffService
    {
        private readonly HieuThuocDbContext _context;
        public StaffService(HieuThuocDbContext context) => _context = context;

        public async Task<StaffIndexViewModel> GetStaffIndexDataAsync(string search)
        {
            var model = new StaffIndexViewModel();

            // 1. Query danh sách nhân viên join với TaiKhoan và VaiTro
            var query = _context.NhanViens
                .Include(n => n.TaiKhoan)
                    .ThenInclude(t => t.MaVaiTroNavigation)
                .AsQueryable();

            // 2. Logic tìm kiếm
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(n => n.HoTen.Contains(search) || n.MaNv.Contains(search) || n.SoDienThoai.Contains(search));
            }

            model.DanhSachNhanVien = await query.OrderByDescending(n => n.MaNv).ToListAsync();

            // 3. Tính toán thống kê dựa trên dữ liệu thật SQL
            model.TongNhanSu = model.DanhSachNhanVien.Count;
            model.DangLamViec = model.DanhSachNhanVien.Count(n => n.TrangThai == true);
            model.TamNghi = model.DanhSachNhanVien.Count(n => n.TrangThai == false);

            // Dược sĩ chính là những người có MaVaiTro = VT02
            model.DuocSiChinh = model.DanhSachNhanVien
                .Count(n => n.TaiKhoan != null && n.TaiKhoan.MaVaiTro == RoleId.DuocSi);

            return model;
        }
    }
}