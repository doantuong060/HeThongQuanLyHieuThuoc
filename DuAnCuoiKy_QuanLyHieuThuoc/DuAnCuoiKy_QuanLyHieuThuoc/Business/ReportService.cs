using Microsoft.EntityFrameworkCore;
using DuAnCuoiKy_QuanLyHieuThuoc.Models;
using DuAnCuoiKy_QuanLyHieuThuoc.Models.ViewModels;
using DuAnCuoiKy_QuanLyHieuThuoc.Enums;

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

            // ============================================================
            // 1. THẺ TÀI CHÍNH — Tháng hiện tại
            // ============================================================
            model.DoanhThuThang = await _context.VwTongTienHoaDons
                .Where(h => h.NgayBan.Month == now.Month && h.NgayBan.Year == now.Year)
                .SumAsync(h => (decimal?)h.TongTien) ?? 0;

            model.SoHoaDon = await _context.HoaDons
                .CountAsync(h => h.NgayBan.Month == now.Month && h.NgayBan.Year == now.Year);

            model.GiaTriNhapKho = await _context.VwTongTienPhieuNhaps
                .Where(p => p.NgayNhap.Month == now.Month && p.NgayNhap.Year == now.Year)
                .SumAsync(p => (decimal?)p.TongTien) ?? 0;

            model.LoiNhuan = model.DoanhThuThang - model.GiaTriNhapKho;

            // ============================================================
            // 2. BIỂU ĐỒ CỘT — Doanh thu 12 tháng gần nhất
            // ============================================================
            for (int i = 11; i >= 0; i--)
            {
                var monthDate = now.AddMonths(-i);
                model.Labels12Thang.Add("T" + monthDate.Month);

                var val = await _context.VwTongTienHoaDons
                    .Where(h => h.NgayBan.Month == monthDate.Month
                             && h.NgayBan.Year == monthDate.Year)
                    .SumAsync(h => (decimal?)h.TongTien) ?? 0;

                // Đơn vị triệu (VD: 1.500.000 → 1.5)
                model.DataDoanhThu.Add(Math.Round(val / 1_000_000m, 1));
            }

            // ============================================================
            // 3. BIỂU ĐỒ TRÒN — Cơ cấu loại sản phẩm (query thật)
            // ============================================================
            var tongSP = await _context.SanPhams.CountAsync(s => s.TrangThai);

            if (tongSP > 0)
            {
                var groups = await _context.SanPhams
                    .Where(s => s.TrangThai)
                    .GroupBy(s => s.LoaiSp)
                    .Select(g => new { LoaiSp = g.Key, SoLuong = g.Count() })
                    .ToListAsync();

                // Bảng màu theo loại — khớp với màu Bootstrap dùng trong View
                var mauMap = new Dictionary<string, string>
                {
                    { ProductType.Thuoc, "#0d6efd" },   // xanh dương
                    { ProductType.VatTu, "#0dcaf0" }    // xanh lam
                };

                var tenMap = new Dictionary<string, string>
                {
                    { ProductType.Thuoc, "Thuốc" },
                    { ProductType.VatTu, "Vật tư y tế" }
                };

                model.CoCauThuoc = groups
                    .OrderByDescending(g => g.SoLuong)
                    .Select(g => new CategoryStat
                    {
                        Ten = tenMap.GetValueOrDefault(g.LoaiSp, g.LoaiSp),
                        PhanTram = Math.Round((double)g.SoLuong / tongSP * 100, 1),
                        Mau = mauMap.GetValueOrDefault(g.LoaiSp, "#adb5bd")
                    })
                    .ToList();
            }
            else
            {
                // Fallback khi chưa có dữ liệu sản phẩm
                model.CoCauThuoc = new List<CategoryStat>
                {
                    new() { Ten = "Thuốc",        PhanTram = 0, Mau = "#0d6efd" },
                    new() { Ten = "Vật tư y tế",  PhanTram = 0, Mau = "#0dcaf0" }
                };
            }

            // ============================================================
            // 4. CẢNH BÁO HẠN DÙNG — Lô sắp hết hạn ≤ 30 ngày
            // ============================================================
            model.SoLoSapHetHan = await _context.VwKiemTraHanDungs
                .CountAsync(l => l.SoNgayConLai <= 30);

            // Giá trị tồn kho của các lô sắp/đã hết hạn (SoLuongConLai × GiaNhap)
            model.GiaTriKhoSapHetHan = await _context.VwKiemTraHanDungs
                .Where(v => v.SoNgayConLai <= 30)
                .Join(
                    _context.LoHangs,
                    v => v.SoLo,
                    l => l.SoLo,
                    (v, l) => (decimal)v.SoLuongConLai * l.GiaNhap
                )
                .SumAsync(x => (decimal?)x) ?? 0;

            return model;
        }
    }
}