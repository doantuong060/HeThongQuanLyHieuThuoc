using Microsoft.EntityFrameworkCore;
using DuAnCuoiKy_QuanLyHieuThuoc.Models;
using DuAnCuoiKy_QuanLyHieuThuoc.Models.ViewModels;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Business
{
    public class SupplierService : ISupplierService
    {
        private readonly HieuThuocDbContext _context;
        public SupplierService(HieuThuocDbContext context) => _context = context;

        public async Task<SupplierIndexViewModel> GetIndexDataAsync(string search)
        {
            var model = new SupplierIndexViewModel();

            // 1. Query cơ bản kèm Join địa lý
            var query = _context.NhaCungCaps
                .Include(n => n.MaPhuongXaNavigation)
                    .ThenInclude(p => p.MaTinhThanhNavigation)
                .AsQueryable();

            // 2. Lọc theo từ khóa tìm kiếm
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(n => n.TenNcc.Contains(search) || n.MaNcc.Contains(search));
            }

            model.AllSuppliers = await query.OrderBy(n => n.MaNcc).ToListAsync();

            // 3. Lấy 3 NCC tiêu biểu (Logic: lấy 3 ông có nhiều phiếu nhập nhất hoặc đơn giản là 3 ông đầu)
            model.FeaturedSuppliers = model.AllSuppliers.Take(3).ToList();

            // 4. Thống kê
            model.TongSoNCC = model.AllSuppliers.Count;
            model.NCCDangHoatDong = model.AllSuppliers.Count(n => n.TrangThai == true);

            return model;
        }
    }
}