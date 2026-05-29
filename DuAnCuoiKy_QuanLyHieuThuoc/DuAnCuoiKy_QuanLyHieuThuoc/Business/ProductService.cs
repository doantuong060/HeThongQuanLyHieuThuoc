using Microsoft.EntityFrameworkCore;
using DuAnCuoiKy_QuanLyHieuThuoc.Models;
using DuAnCuoiKy_QuanLyHieuThuoc.Models.ViewModels;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Business
{
    public class ProductService : IProductService
    {
        private readonly HieuThuocDbContext _context;
        public ProductService(HieuThuocDbContext context) => _context = context;

        public async Task<ProductIndexViewModel> GetProductIndexDataAsync(string search, string loai)
        {
            var model = new ProductIndexViewModel();

            // 1. Lấy dữ liệu từ View SQL (vw_TonKhoSanPham)
            var query = _context.VwTonKhoSanPhams.AsQueryable();

            // 2. Logic Tìm kiếm & Lọc
            if (!string.IsNullOrEmpty(search))
                query = query.Where(p => p.TenSp.Contains(search) || p.MaSp.Contains(search));

            if (!string.IsNullOrEmpty(loai) && loai != "Tất cả")
                query = query.Where(p => p.LoaiSp == loai);

            model.Products = await query.OrderBy(p => p.MaSp).ToListAsync();

            // 3. Tính toán thống kê
            model.TongSanPham = model.Products.Count;
            model.SoLuongSapHet = model.Products.Count(p => p.SoLuongTon <= p.MucCanhBao);
            model.SoLuongSapHetHan = await _context.VwKiemTraHanDungs.CountAsync(l => l.SoNgayConLai <= 30);

            return model;
        }
    }
}