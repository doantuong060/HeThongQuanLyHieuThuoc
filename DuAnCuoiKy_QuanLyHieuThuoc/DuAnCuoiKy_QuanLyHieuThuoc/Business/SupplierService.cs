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
        public async Task<bool> AddSupplierAsync(AddSupplierViewModel model)
        {
            try
            {
                // 1. Logic tự động sinh mã NCC (Ví dụ: NCC0011)
                var lastNcc = await _context.NhaCungCaps
                    .OrderByDescending(n => n.MaNcc)
                    .FirstOrDefaultAsync();

                int nextId = 1;
                if (lastNcc != null)
                {
                    // Cắt chuỗi NCC0001 -> lấy số 1 và cộng thêm 1
                    string numericPart = lastNcc.MaNcc.Replace("NCC", "");
                    if (int.TryParse(numericPart, out int lastId))
                    {
                        nextId = lastId + 1;
                    }
                }
                string newMaNcc = "NCC" + nextId.ToString("D4"); // Định dạng 4 chữ số

                // 2. Tạo đối tượng Entity (Dựa theo SQL v8)
                var ncc = new NhaCungCap
                {
                    MaNcc = newMaNcc,
                    TenNcc = model.TenNcc,
                    SoDienThoai = model.SoDienThoai,
                    Email = model.Email,
                    DiaChi = model.DiaChiChiTiet,
                    // Lưu ý: Cột MaPhuongXa trong SQL cho phép NULL
                    // Vì UI chỉ chọn Tỉnh, ta có thể để MaPhuongXa là null hoặc xử lý logic thêm
                    MaPhuongXa = null,
                    TrangThai = true // Mặc định hoạt động
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
    }
}