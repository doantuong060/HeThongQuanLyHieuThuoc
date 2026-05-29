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

        // DÁN ACTION INDEX VÀO ĐÂY
        public IActionResult Index()
        {
            var dsLoHang = _context.LoHangs
                .Include(x => x.MaSpNavigation)
                .ThenInclude(x => x.MaDvtNavigation)
                .ToList();

            var ngayCanhBaoHSD =
                DateOnly.FromDateTime(DateTime.Now.AddMonths(1));

            var result = dsLoHang.Select(x =>
            {
                int nguong =
                    x.MaSpNavigation?.MucCanhBao ?? 20;

                int soLuongCon =
                    x.SoLuongConLai;

                int progress =
                    nguong == 0
                    ? 100
                    : (int)Math.Min(
                        (double)soLuongCon / nguong * 100,
                        100);

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
                    Loai = x.MaSpNavigation?.LoaiSp ?? "",
                    DVT = x.MaSpNavigation?.MaDvtNavigation?.TenDvt ?? "",
                    Ton = soLuongCon,
                    Nguong = nguong,
                    LoGanHSD =
                        x.SoLo + " - " +
                        x.HanSuDung.ToString("dd/MM/yyyy"),
                    Status = status,
                    Class = css,
                    Progress = progress
                };
            }).ToList();

            ViewBag.DsTonKho = result;

            ViewBag.TongMaThuoc =
                result.Select(x => x.Ma)
                      .Distinct()
                      .Count();

            ViewBag.AnToanCount =
                result.Count(x => x.Status == "An toàn");

            ViewBag.SapHetHangCount =
                result.Count(x => x.Status == "Sắp hết");

            ViewBag.NguyCapCount =
                result.Count(x => x.Status == "Sắp hết hạn");

            return View();
        }
    }
}