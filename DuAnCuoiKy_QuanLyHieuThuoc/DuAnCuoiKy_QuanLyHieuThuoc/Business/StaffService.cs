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

        // ============================================================
        // LẤY DANH SÁCH NHÂN VIÊN
        // ============================================================
        public async Task<StaffIndexViewModel> GetStaffIndexDataAsync(string search)
        {
            var model = new StaffIndexViewModel();

            var query = _context.NhanViens
                .Include(n => n.TaiKhoan)
                    .ThenInclude(t => t.MaVaiTroNavigation)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(n =>
                    n.HoTen.Contains(search) ||
                    n.MaNv.Contains(search) ||
                    n.SoDienThoai.Contains(search));
            }

            model.DanhSachNhanVien = await query.OrderByDescending(n => n.MaNv).ToListAsync();
            model.TongNhanSu = model.DanhSachNhanVien.Count;
            model.DangLamViec = model.DanhSachNhanVien.Count(n => n.TrangThai == true);
            model.TamNghi = model.DanhSachNhanVien.Count(n => n.TrangThai == false);
            model.DuocSiChinh = model.DanhSachNhanVien
                .Count(n => n.TaiKhoan != null && n.TaiKhoan.MaVaiTro == RoleId.DuocSi);

            return model;
        }

        // ============================================================
        // THÊM NHÂN VIÊN MỚI
        // ============================================================
        public async Task<bool> AddStaffAsync(AddStaffViewModel model)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Tự động sinh MaNV an toàn
                var lastNV = await _context.NhanViens
                    .OrderByDescending(n => n.MaNv)
                    .FirstOrDefaultAsync();

                int nextId = 1;
                if (lastNV != null && lastNV.MaNv.Length > 2)
                {
                    if (int.TryParse(lastNV.MaNv.Substring(2), out int parsed))
                        nextId = parsed + 1;
                }
                string newMaNV = "NV" + nextId.ToString("D4");

                // NgaySinh nullable - fallback an toàn tránh DateTime.MinValue
                DateOnly ngaySinh = model.NgaySinh.HasValue
                    ? DateOnly.FromDateTime(model.NgaySinh.Value)
                    : new DateOnly(1990, 1, 1);

                var nv = new NhanVien
                {
                    MaNv = newMaNV,
                    HoTen = model.HoTen,
                    GioiTinh = model.GioiTinh,
                    NgaySinh = ngaySinh,
                    SoDienThoai = model.SoDienThoai,
                    Email = model.Email,
                    NgayVaoLam = DateOnly.FromDateTime(DateTime.Now),
                    TrangThai = true
                };
                _context.NhanViens.Add(nv);

                var tk = new TaiKhoan
                {
                    MaNv = newMaNV,
                    TenDangNhap = model.TenDangNhap,
                    MatKhau = model.MatKhau,   // Phải đúng rule: CHỮ HOA + chữ thường + @
                    MaVaiTro = model.MaVaiTro,
                    NgayTao = DateTime.Now
                };
                _context.TaiKhoans.Add(tk);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        // ============================================================
        // LẤY DỮ LIỆU NHÂN VIÊN ĐỂ ĐỔ VÀO FORM SỬA
        // ============================================================
        public async Task<EditStaffViewModel?> GetStaffForEditAsync(string maNv)
        {
            var nv = await _context.NhanViens
                .Include(n => n.TaiKhoan)
                .FirstOrDefaultAsync(n => n.MaNv == maNv);

            if (nv == null) return null;

            return new EditStaffViewModel
            {
                MaNv = nv.MaNv,
                HoTen = nv.HoTen,
                GioiTinh = nv.GioiTinh,
                NgaySinh = nv.NgaySinh.ToDateTime(TimeOnly.MinValue),
                SoDienThoai = nv.SoDienThoai,
                Email = nv.Email,
                MaVaiTro = nv.TaiKhoan?.MaVaiTro ?? ""
            };
        }

        // ============================================================
        // CẬP NHẬT THÔNG TIN NHÂN VIÊN
        // ============================================================
        public async Task<bool> UpdateStaffAsync(EditStaffViewModel model)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var nv = await _context.NhanViens
                    .Include(n => n.TaiKhoan)
                    .FirstOrDefaultAsync(n => n.MaNv == model.MaNv);

                if (nv == null) return false;

                // Cập nhật bảng NhanVien
                nv.HoTen = model.HoTen;
                nv.GioiTinh = model.GioiTinh;
                nv.SoDienThoai = model.SoDienThoai;
                nv.Email = model.Email;

                if (model.NgaySinh.HasValue)
                    nv.NgaySinh = DateOnly.FromDateTime(model.NgaySinh.Value);

                // Cập nhật bảng TaiKhoan
                if (nv.TaiKhoan != null)
                {
                    nv.TaiKhoan.MaVaiTro = model.MaVaiTro;

                    // Chỉ cập nhật mật khẩu nếu user nhập mới
                    if (!string.IsNullOrWhiteSpace(model.MatKhauMoi))
                        nv.TaiKhoan.MatKhau = model.MatKhauMoi;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }
    }
}