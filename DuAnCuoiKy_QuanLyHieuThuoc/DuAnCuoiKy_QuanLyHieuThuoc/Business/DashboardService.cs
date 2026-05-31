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
            // BUG FIX #1: DateTime.Today chỉ lấy phần ngày (00:00:00), nhưng NgayBan
            // trong DB là datetime đầy đủ. Cần lấy đến cuối ngày hôm nay.
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            var model = new AdminDashboardViewModel();

            // 1. Thống kê từ SQL Views
            // BUG FIX #2: Điều kiện cũ ">= today" đúng, nhưng cần thêm "< tomorrow"
            // để không lấy sang ngày mai khi có dữ liệu đúng 00:00:00 ngày mai.
            model.DoanhThuHomNay = await _context.VwTongTienHoaDons
                .Where(h => h.NgayBan >= today && h.NgayBan < tomorrow)
                .SumAsync(h => (decimal?)h.TongTien) ?? 0m;

            model.SoHoaDonHomNay = await _context.HoaDons
                .CountAsync(h => h.NgayBan >= today && h.NgayBan < tomorrow);

            // BUG FIX #3: VwTonKhoSanPham và VwKiemTraHanDung đều HasNoKey() (View).
            // EF Core không tự cache kết quả - mỗi lần gọi là 1 query riêng.
            // Để tránh nhiều roundtrip, tải về List trước rồi xử lý in-memory.
            var tonKhoList = await _context.VwTonKhoSanPhams.ToListAsync();
            var hanDungList = await _context.VwKiemTraHanDungs.ToListAsync();

            model.ThuocSapHetHang = tonKhoList.Count(t => t.SoLuongTon <= t.MucCanhBao);

            // BUG FIX #4: Logic cũ chỉ đếm SoNgayConLai <= 30, nhưng bỏ sót
            // lô đã quá hạn (SoNgayConLai < 0). Cần đếm cả lô quá hạn.
            model.LoSapHetHan = hanDungList.Count(l => l.SoNgayConLai <= 30);

            // 2. Lấy 5 hóa đơn mới nhất
            model.HoaDonMoiNhat = await _context.VwTongTienHoaDons
                .OrderByDescending(h => h.NgayBan)
                .Take(5)
                .ToListAsync();

            // 3. Lấy 5 mặt hàng cần nhập gấp (dùng list đã tải)
            model.DanhSachCanhBao = tonKhoList
                .Where(t => t.SoLuongTon <= t.MucCanhBao)
                .OrderBy(t => t.SoLuongTon)
                .Take(5)
                .ToList();

            // 4. Biểu đồ doanh thu 6 tháng - tính từ server để đảm bảo đúng múi giờ
            for (int i = 5; i >= 0; i--)
            {
                var date = DateTime.Today.AddMonths(-i);
                var label = $"T{date.Month}/{date.Year % 100}"; // Hiển thị "T5/26" rõ hơn "T5"
                var total = await _context.VwTongTienHoaDons
                    .Where(h => h.NgayBan.Month == date.Month && h.NgayBan.Year == date.Year)
                    .SumAsync(h => (decimal?)h.TongTien) ?? 0m;

                model.LabelsBieuDo.Add(label);
                model.DataBieuDo.Add(Math.Round(total / 1_000_000, 1)); // 1 chữ số thập phân
            }

            return model;
        }
    }
}