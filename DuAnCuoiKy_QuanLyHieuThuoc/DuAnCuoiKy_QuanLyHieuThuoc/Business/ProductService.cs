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
        public async Task<bool> AddProductAsync(AddProductViewModel model)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Tự động sinh MaSP dựa trên LoaiSP (Chuẩn SQL)
                string prefix = model.LoaiSp == "THUOC" ? "TH" : "VT";
                var lastSp = await _context.SanPhams
                    .Where(s => s.MaSp.StartsWith(prefix))
                    .OrderByDescending(s => s.MaSp)
                    .FirstOrDefaultAsync();

                int nextId = (lastSp != null) ? int.Parse(lastSp.MaSp.Substring(2)) + 1 : 1;
                string newMaSp = prefix + nextId.ToString("D4");

                // 2. Lưu vào bảng SanPham
                var sp = new SanPham
                {
                    MaSp = newMaSp,
                    TenSp = model.TenSp,
                    MaDvt = model.MaDvt,
                    GiaBan = model.GiaBan,
                    MucCanhBao = model.MucCanhBao,
                    LoaiSp = model.LoaiSp,
                    TrangThai = true
                };
                _context.SanPhams.Add(sp);

                // 3. Lưu vào bảng con tương ứng
                if (model.LoaiSp == "THUOC")
                {
                    var thuoc = new Thuoc
                    {
                        MaSp = newMaSp,
                        MaLoai = model.MaLoai!,
                        CanToa = model.CanToa,
                        GhiChu = model.GhiChu
                    };
                    _context.Thuocs.Add(thuoc);
                }
                else
                {
                    var vtyt = new VatTuYte
                    {
                        MaSp = newMaSp,
                        MaLoaiVt = model.MaLoaiVt!,
                        NhaSanXuat = model.NhaSanXuat
                    };
                    _context.VatTuYtes.Add(vtyt);
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