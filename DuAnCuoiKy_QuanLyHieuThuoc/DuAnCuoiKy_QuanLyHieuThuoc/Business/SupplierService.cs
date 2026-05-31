using Microsoft.EntityFrameworkCore;
using DuAnCuoiKy_QuanLyHieuThuoc.Models;
using DuAnCuoiKy_QuanLyHieuThuoc.Models.ViewModels;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Business
{
    public class SupplierService : ISupplierService
    {
        private readonly HieuThuocDbContext _context;
        public SupplierService(HieuThuocDbContext context) => _context = context;

        // ============================================================
        // LẤY DỮ LIỆU INDEX
        // ============================================================
        public async Task<SupplierIndexViewModel> GetIndexDataAsync(string search)
        {
            var model = new SupplierIndexViewModel();

            var query = _context.NhaCungCaps
                .Include(n => n.MaPhuongXaNavigation)
                    .ThenInclude(p => p.MaTinhThanhNavigation)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(n => n.TenNcc.Contains(search) || n.MaNcc.Contains(search));

            model.AllSuppliers = await query.OrderBy(n => n.MaNcc).ToListAsync();
            model.FeaturedSuppliers = model.AllSuppliers.Take(3).ToList();
            model.TongSoNCC = model.AllSuppliers.Count;
            model.NCCDangHoatDong = model.AllSuppliers.Count(n => n.TrangThai == true);

            return model;
        }

        // ============================================================
        // THÊM MỚI
        // ============================================================
        public async Task<bool> AddSupplierAsync(AddSupplierViewModel model)
        {
            try
            {
                var lastNcc = await _context.NhaCungCaps
                    .OrderByDescending(n => n.MaNcc.Length)
                    .ThenByDescending(n => n.MaNcc)
                    .FirstOrDefaultAsync();

                int nextId = 1;
                if (lastNcc != null)
                {
                    string soThuTu = lastNcc.MaNcc.Replace("NCC", "");
                    if (int.TryParse(soThuTu, out int lastId))
                        nextId = lastId + 1;
                }

                var ncc = new NhaCungCap
                {
                    MaNcc = "NCC" + nextId.ToString("D4"),
                    TenNcc = model.TenNcc,
                    SoDienThoai = model.SoDienThoai,
                    Email = model.Email,
                    DiaChi = model.DiaChiChiTiet,
                    MaPhuongXa = null,
                    TrangThai = true
                };

                _context.NhaCungCaps.Add(ncc);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // ============================================================
        // CẬP NHẬT
        // ============================================================
        public async Task<bool> UpdateSupplierAsync(EditSupplierViewModel model)
        {
            try
            {
                var ncc = await _context.NhaCungCaps.FindAsync(model.MaNcc);
                if (ncc == null) return false;

                ncc.TenNcc = model.TenNcc;
                ncc.SoDienThoai = model.SoDienThoai;
                ncc.Email = model.Email;
                ncc.DiaChi = model.DiaChiChiTiet;
                ncc.TrangThai = model.TrangThai;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // ============================================================
        // XÓA
        // ============================================================
        public async Task<bool> DeleteSupplierAsync(string maNcc)
        {
            try
            {
                var ncc = await _context.NhaCungCaps
                    .Include(n => n.PhieuNhaps)
                    .FirstOrDefaultAsync(n => n.MaNcc == maNcc);

                if (ncc == null) return false;

                // Không xóa nếu đã có phiếu nhập liên quan
                if (ncc.PhieuNhaps.Any()) return false;

                _context.NhaCungCaps.Remove(ncc);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}