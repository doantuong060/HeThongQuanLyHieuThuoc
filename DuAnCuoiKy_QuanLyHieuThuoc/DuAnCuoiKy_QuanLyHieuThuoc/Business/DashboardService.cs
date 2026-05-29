using Microsoft.EntityFrameworkCore;
using DuAnCuoiKy_QuanLyHieuThuoc.Models;
using DuAnCuoiKy_QuanLyHieuThuoc.Models.ViewModels;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Business
{
    public class DashboardService : IDashboardService
    {
        private readonly HieuThuocDbContext _context;
        public DashboardService(HieuThuocDbContext context) => _context = context;

        public async Task<AdminDashboardViewModel> GetAdminDashboardDataAsync()
        {
            var today = DateTime.Today;
            var model = new AdminDashboardViewModel();

            // 1. Thống kê từ SQL Views
            model.DoanhThuHomNay = await _context.VwTongTienHoaDons
                .Where(h => h.NgayBan >= today)
                .SumAsync(h => h.TongTien);

            model.SoHoaDonHomNay = await _context.HoaDons
                .CountAsync(h => h.NgayBan >= today);

            model.ThuocSapHetHang = await _context.VwTonKhoSanPhams
                .CountAsync(t => t.SoLuongTon <= t.MucCanhBao);

            model.LoSapHetHan = await _context.VwKiemTraHanDungs
                .CountAsync(l => l.SoNgayConLai <= 30);

            // 2. Lấy 5 hóa đơn mới nhất (Dùng View để có đầy đủ tên NV, KH)
            model.HoaDonMoiNhat = await _context.VwTongTienHoaDons
                .OrderByDescending(h => h.NgayBan)
                .Take(5)
                .ToListAsync();

            // 3. Lấy 5 mặt hàng cần nhập gấp
            model.DanhSachCanhBao = await _context.VwTonKhoSanPhams
                .Where(t => t.SoLuongTon <= t.MucCanhBao)
                .OrderBy(t => t.SoLuongTon)
                .Take(5)
                .ToListAsync();

            // 4. Logic Biểu đồ doanh thu 6 tháng gần nhất
            for (int i = 5; i >= 0; i--)
            {
                var date = DateTime.Today.AddMonths(-i);
                var label = $"T{date.Month}";
                var total = await _context.VwTongTienHoaDons
                    .Where(h => h.NgayBan.Month == date.Month && h.NgayBan.Year == date.Year)
                    .SumAsync(h => h.TongTien);

                model.LabelsBieuDo.Add(label);
                model.DataBieuDo.Add(total / 1000000); // Đơn vị triệu đồng
            }

            return model;
        }
    }
}