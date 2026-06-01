using DuAnCuoiKy_QuanLyHieuThuoc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Controllers
{
    public class LoHangsController : Controller
    {
        private readonly HieuThuocDbContext _context;

        public LoHangsController(HieuThuocDbContext context)
        {
            _context = context;
        }

        // Thêm tham số loai để lọc THUOC / VATTU
        public IActionResult Index(string? tab, string? keyword, string? loai)
        {
            var dsLoHang = _context.LoHangs
                .Include(x => x.MaSpNavigation)
                    .ThenInclude(x => x.MaDvtNavigation)
                .ToList();

            var ngayCanhBaoHSD = DateOnly.FromDateTime(DateTime.Now.AddMonths(1));

            var result = dsLoHang.Select(x =>
            {
                int nguong = x.MaSpNavigation?.MucCanhBao ?? 20;
                int soLuongCon = x.SoLuongConLai;
                int progress = nguong == 0
                    ? 100
                    : (int)Math.Min((double)soLuongCon / nguong * 100, 100);

                string status = "An toàn";
                string css = "success";

                if (soLuongCon <= nguong)
                {
                    status = "Sắp hết";
                    css = "warning";
                }
                if (x.HanSuDung <= ngayCanhBaoHSD)
                {
                    status = "Sắp hết hạn";
                    css = "danger";
                }

                return new
                {
                    Ma = x.MaSp,
                    Ten = x.MaSpNavigation?.TenSp ?? "",
                    // LoaiSP dùng để lọc Thuốc / Vật tư ("THUOC" hoặc "VATTU")
                    LoaiSP = x.MaSpNavigation?.LoaiSp ?? "",
                    // Loai là nhóm chi tiết (Kháng sinh, Băng gạc...)
                    Loai = x.MaSpNavigation?.LoaiSp ?? "",
                    DVT = x.MaSpNavigation?.MaDvtNavigation?.TenDvt ?? "",
                    Ton = soLuongCon,
                    Nguong = nguong,
                    LoGanHSD = x.SoLo + " - " + x.HanSuDung.ToString("dd/MM/yyyy"),
                    Status = status,
                    Class = css,
                    Progress = progress
                };
            }).ToList();

            // ── Đếm cho sidebar (tính trên toàn bộ, không bị filter ảnh hưởng) ──
            ViewBag.TongMaThuoc = result.Select(x => x.Ma).Distinct().Count();
            ViewBag.AnToanCount = result.Count(x => x.Status == "An toàn");
            ViewBag.SapHetHangCount = result.Count(x => x.Status == "Sắp hết");
            ViewBag.NguyCapCount = result.Count(x => x.Status == "Sắp hết hạn");

            // ── Đếm riêng Thuốc / Vật tư ──
            ViewBag.SoThuoc = result
                .DistinctBy(x => x.Ma)
                .Count(x => x.LoaiSP == "THUOC");
            ViewBag.SoVatTu = result
                .DistinctBy(x => x.Ma)
                .Count(x => x.LoaiSP == "VATTU");

            // ── Lọc theo tab (trạng thái) ──
            var filtered = result.AsEnumerable();

            if (tab == "saphet")
                filtered = filtered.Where(x => x.Status == "Sắp hết");
            else if (tab == "saphetan")
                filtered = filtered.Where(x => x.Status == "Sắp hết hạn");

            // ── Lọc theo loại SP ──
            if (loai == "THUOC")
                filtered = filtered.Where(x => x.LoaiSP == "THUOC");
            else if (loai == "VATTU")
                filtered = filtered.Where(x => x.LoaiSP == "VATTU");

            // ── Lọc theo keyword ──
            if (!string.IsNullOrWhiteSpace(keyword))
                filtered = filtered.Where(x =>
                    x.Ma.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                    x.Ten.Contains(keyword, StringComparison.OrdinalIgnoreCase));

            ViewBag.DsTonKho = filtered.ToList();
            ViewBag.ActiveTab = tab ?? "all";
            ViewBag.ActiveLoai = loai ?? "all";
            ViewBag.Keyword = keyword ?? "";

            return View();
        }
    }
}