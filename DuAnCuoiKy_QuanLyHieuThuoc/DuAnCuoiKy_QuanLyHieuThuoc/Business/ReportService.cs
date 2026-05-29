using Microsoft.EntityFrameworkCore;
using DuAnCuoiKy_QuanLyHieuThuoc.Models;
using DuAnCuoiKy_QuanLyHieuThuoc.Models.ViewModels;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Business
{
    public class ReportService : IReportService
    {
        private readonly HieuThuocDbContext _context;
        public ReportService(HieuThuocDbContext context) => _context = context;

        public async Task<ReportViewModel> GetFullReportAsync()
        {
            var model = new ReportViewModel();
            var now = DateTime.Now;

            // 1. Tính toán thẻ tài chính (Tháng hiện tại)
            model.DoanhThuThang = await _context.VwTongTienHoaDons
                .Where(h => h.NgayBan.Month == now.Month && h.NgayBan.Year == now.Year)
                .SumAsync(h => h.TongTien);

            model.SoHoaDon = await _context.HoaDons
                .CountAsync(h => h.NgayBan.Month == now.Month && h.NgayBan.Year == now.Year);

            model.GiaTriNhapKho = await _context.VwTongTienPhieuNhaps
                .Where(p => p.NgayNhap.Month == now.Month && p.NgayNhap.Year == now.Year)
                .SumAsync(p => p.TongTien);

            model.LoiNhuan = model.DoanhThuThang - model.GiaTriNhapKho;

            // 2. Lấy dữ liệu biểu đồ 12 tháng gần nhất
            for (int i = 11; i >= 0; i--)
            {
                var monthDate = now.AddMonths(-i);
                model.Labels12Thang.Add("T" + monthDate.Month);
                var val = await _context.VwTongTienHoaDons
                    .Where(h => h.NgayBan.Month == monthDate.Month && h.NgayBan.Year == monthDate.Year)
                    .SumAsync(h => h.TongTien);
                model.DataDoanhThu.Add(val / 1000000); // Đơn vị triệu
            }

            // 3. Cơ cấu loại thuốc (Dựa trên bảng SanPham và LoaiThuoc)
            // Giả lập logic hoặc query group by
            model.CoCauThuoc = new List<CategoryStat> {
                new() { Ten = "Thuốc kê đơn", PhanTram = 45, Mau = "#0d6efd" },
                new() { Ten = "Thực phẩm CN", PhanTram = 30, Mau = "#4b5563" },
                new() { Ten = "Dược mỹ phẩm", PhanTram = 15, Mau = "#0dcaf0" },
                new() { Ten = "Vật tư y tế", PhanTram = 10, Mau = "#adb5bd" }
            };

            // 4. Cảnh báo AI
            model.SoLoSapHetHan = await _context.VwKiemTraHanDungs.CountAsync(l => l.SoNgayConLai <= 30);

            return model;
        }
    }
}