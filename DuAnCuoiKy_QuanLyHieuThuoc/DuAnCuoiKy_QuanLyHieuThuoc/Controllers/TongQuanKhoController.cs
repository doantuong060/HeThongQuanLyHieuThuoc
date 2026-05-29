using DuAnCuoiKy_QuanLyHieuThuoc.Models;
using DuAnCuoiKy_QuanLyHieuThuoc.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Controllers
{
    public class TongQuanKhoController : Controller
    {
        private readonly HieuThuocDbContext _context;

        public TongQuanKhoController(HieuThuocDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            TongQuanKhoViewModel vm = new TongQuanKhoViewModel();

            // ==================================================
            // THẺ THỐNG KÊ
            // ==================================================

            vm.TongMatHang = _context.SanPhams.Count();

            vm.SapHetHang = _context.VwTonKhoSanPhams
                .Count(x => x.SoLuongTon <= x.MucCanhBao);

            vm.SapHetHan = _context.VwKiemTraHanDungs
                .Count(x => x.SoNgayConLai <= 30);

            // ==================================================
            // THUỐC CẦN NHẬP
            // ==================================================

            vm.DsCanNhap = _context.VwTonKhoSanPhams
                .Where(x => x.SoLuongTon <= x.MucCanhBao)
                .OrderBy(x => x.SoLuongTon)
                .Take(5)
                .Select(x => new ThuocCanNhapVM
                {
                    TenThuoc = x.TenSp,
                    SoLuongTon = x.SoLuongTon,
                    MucCanhBao = x.MucCanhBao,
                    DonViTinh = x.TenDvt ?? ""
                })
                .ToList();

            // ==================================================
            // LÔ SẮP HẾT HẠN
            // ==================================================

            vm.DsHetHan = _context.VwKiemTraHanDungs
                .Where(x => x.SoNgayConLai <= 30)
                .OrderBy(x => x.SoNgayConLai)
                .Take(5)
                .Select(x => new LoSapHetHanVM
                {
                    TenThuoc = x.TenSp,
                    SoLo = x.SoLo,
                    HanSuDung = x.HanSuDung,
                    SoNgayConLai = x.SoNgayConLai ?? 0,

                    TrangThai =
                        (x.SoNgayConLai ?? 0) <= 0
                        ? "Hết hạn"
                        : "Sắp hết hạn",

                    CssClass =
                        (x.SoNgayConLai ?? 0) <= 0
                        ? "danger"
                        : "warning"
                })
                .ToList();

            // ==================================================
            // PHIẾU NHẬP GẦN ĐÂY
            // ==================================================

            vm.DsPhieuNhap = _context.PhieuNhaps
                .Include(x => x.MaNccNavigation)
                .OrderByDescending(x => x.NgayNhap)
                .Take(5)
                .Select(x => new PhieuNhapGanDayVM
                {
                    MaPhieuNhap = x.MaPhieuNhap,
                    NgayNhap = x.NgayNhap,
                    NhaCungCap = x.MaNccNavigation.TenNcc,
                    TongTien = x.TongTien ?? 0
                })
                .ToList();

            return View(vm);
        }
    }
}