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

            // 1. Tính toán thống kê trên toàn bộ dữ liệu hệ thống (Chưa bị lọc bởi tìm kiếm)
            var baseTonKhoQuery = _context.VwTonKhoSanPhams;

            model.TongSanPham = await baseTonKhoQuery.CountAsync();
            model.SoLuongSapHet = await baseTonKhoQuery.CountAsync(p => p.SoLuongTon <= p.MucCanhBao);
            model.SoLuongSapHetHan = await _context.VwKiemTraHanDungs.CountAsync(l => l.SoNgayConLai <= 30);

            // 2. Tạo query riêng phục vụ việc Tìm kiếm & Lọc hiển thị lên bảng dữ liệu
            var displayQuery = baseTonKhoQuery.AsQueryable();

            if (!string.IsNullOrEmpty(search))
                displayQuery = displayQuery.Where(p => p.TenSp.Contains(search) || p.MaSp.Contains(search));

            if (!string.IsNullOrEmpty(loai) && loai != "Tất cả")
                displayQuery = displayQuery.Where(p => p.LoaiSp == loai);

            model.Products = await displayQuery.OrderBy(p => p.MaSp).ToListAsync();

            return model;
        }

        public async Task<bool> AddProductAsync(AddProductViewModel model)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Sinh mã tự động an toàn tuyệt đối (Xử lý ép kiểu bằng C# thay vì SQL)
                string prefix = model.LoaiSp == "THUOC" ? "TH" : "VT";

                // Kéo mã lớn nhất về dưới dạng chuỗi
                var maxMaSp = await _context.SanPhams
                    .Where(s => s.MaSp.StartsWith(prefix) && s.MaSp.Length == 6)
                    .OrderByDescending(s => s.MaSp) // Sắp xếp Z-A để lấy cái to nhất
                    .Select(s => s.MaSp)
                    .FirstOrDefaultAsync();

                int nextId = 1;

                // Nếu trong DB đã có sản phẩm, ta tiến hành cắt chuỗi bằng C#
                if (!string.IsNullOrEmpty(maxMaSp))
                {
                    string numberPart = maxMaSp.Substring(2); // Cắt bỏ "TH" hoặc "VT", lấy 4 số cuối
                    if (int.TryParse(numberPart, out int parsedId))
                    {
                        nextId = parsedId + 1;
                    }
                }

                string newMaSp = prefix + nextId.ToString("D4"); // Format lại thành TH0025, VT0002...

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
            catch (Exception ex)
            {
                Console.WriteLine($"[Lỗi Thêm Sản Phẩm]: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"[Chi tiết lỗi]: {ex.InnerException.Message}");
                }

                await transaction.RollbackAsync();
                return false;
            }
        }
    }
}