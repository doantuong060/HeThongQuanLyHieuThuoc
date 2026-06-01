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
            var tomorrow = today.AddDays(1);

            var model = new AdminDashboardViewModel();

            model.DoanhThuHomNay = await _context.VwTongTienHoaDons
                .Where(h => h.NgayBan >= today && h.NgayBan < tomorrow)
                .SumAsync(h => (decimal?)h.TongTien) ?? 0m;

            model.SoHoaDonHomNay = await _context.HoaDons
                .CountAsync(h => h.NgayBan >= today && h.NgayBan < tomorrow);

            var tonKhoList = await _context.VwTonKhoSanPhams.ToListAsync();
            var hanDungList = await _context.VwKiemTraHanDungs.ToListAsync();

            model.ThuocSapHetHang = tonKhoList.Count(t => t.SoLuongTon <= t.MucCanhBao);

            model.LoSapHetHan = hanDungList.Count(l => l.SoNgayConLai <= 30);

            model.HoaDonMoiNhat = await _context.VwTongTienHoaDons
                .OrderByDescending(h => h.NgayBan)
                .Take(5)
                .ToListAsync();

            model.DanhSachCanhBao = tonKhoList
                .Where(t => t.SoLuongTon <= t.MucCanhBao)
                .OrderBy(t => t.SoLuongTon)
                .Take(5)
                .ToList();

            for (int i = 5; i >= 0; i--)
            {
                var date = DateTime.Today.AddMonths(-i);
                var label = $"T{date.Month}/{date.Year % 100}"; 
                var total = await _context.VwTongTienHoaDons
                    .Where(h => h.NgayBan.Month == date.Month && h.NgayBan.Year == date.Year)
                    .SumAsync(h => (decimal?)h.TongTien) ?? 0m;

                model.LabelsBieuDo.Add(label);
                model.DataBieuDo.Add(Math.Round(total / 1_000_000, 1)); 
            }

            return model;
        }
    }
}