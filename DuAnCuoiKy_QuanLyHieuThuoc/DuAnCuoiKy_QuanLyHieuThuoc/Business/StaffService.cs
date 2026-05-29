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
        public async Task<bool> AddStaffAsync(AddStaffViewModel model)
        {
            // Bắt đầu một Giao dịch (Transaction)
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Tự động sinh MaNV (Ví dụ: NV0001)
                var lastNV = await _context.NhanViens.OrderByDescending(n => n.MaNv).FirstOrDefaultAsync();
                int nextId = (lastNV != null) ? int.Parse(lastNV.MaNv.Substring(2)) + 1 : 1;
                string newMaNV = "NV" + nextId.ToString("D4");

                // 2. Tạo đối tượng NhanVien
                var nv = new NhanVien
                {
                    MaNv = newMaNV,
                    HoTen = model.HoTen,
                    GioiTinh = model.GioiTinh,
                    NgaySinh = DateOnly.FromDateTime(model.NgaySinh),
                    SoDienThoai = model.SoDienThoai,
                    Email = model.Email,
                    NgayVaoLam = DateOnly.FromDateTime(DateTime.Now),
                    TrangThai = true
                };
                _context.NhanViens.Add(nv);

                // 3. Tạo đối tượng TaiKhoan liên kết
                var tk = new TaiKhoan
                {
                    MaNv = newMaNV,
                    TenDangNhap = model.TenDangNhap,
                    MatKhau = model.MatKhau, // Trong thực tế nên mã hóa mật khẩu ở đây
                    MaVaiTro = model.MaVaiTro,
                    NgayTao = DateTime.Now
                };
                _context.TaiKhoans.Add(tk);

                // Lưu tất cả vào SQL
                await _context.SaveChangesAsync();

                // Xác nhận hoàn tất giao dịch
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                // Nếu có lỗi, hủy bỏ mọi thay đổi của cả 2 bảng
                await transaction.RollbackAsync();
                return false;
            }
        }
    }
}