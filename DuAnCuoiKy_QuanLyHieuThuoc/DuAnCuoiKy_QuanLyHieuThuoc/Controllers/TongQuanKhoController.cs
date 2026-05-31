<<<<<<< HEAD
﻿using Microsoft.AspNetCore.Mvc;
using DuAnCuoiKy_QuanLyHieuThuoc.Models;
=======
﻿using DuAnCuoiKy_QuanLyHieuThuoc.Models;
using DuAnCuoiKy_QuanLyHieuThuoc.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
>>>>>>> a1acad9 (Hoan thien giao dien tong quan kho)

namespace DuAnCuoiKy_QuanLyHieuThuoc.Controllers
{
    public class TongQuanKhoController : Controller
    {
<<<<<<< HEAD
        public IActionResult Index()
        {
            // --- [VỊ TRÍ HARDCODE HỆ THỐNG] ---

            // 1. Thống kê 3 thẻ trên cùng (Ảnh 14)
            ViewBag.TongMatHang = 248;
            ViewBag.SapHetHang = 24;
            ViewBag.SapHetHan = 08;

            // 2. Danh sách thuốc cần nhập ngay
            ViewBag.DsCanNhap = new List<dynamic> {
                new { Ten = "Amoxicillin 500mg", Ton = "15 hộp", DinhMuc = "50 hộp" },
                new { Ten = "Paracetamol 500mg", Ton = "20 vỉ", DinhMuc = "100 vỉ" },
                new { Ten = "Vitamin C 1000mg", Ton = "5 lọ", DinhMuc = "30 lọ" }
            };

            // 3. Danh sách lô hàng sắp hết hạn
            ViewBag.DsHetHan = new List<dynamic> {
                new { Ten = "Oresol 245", SoLo = "LOT-2301A", HSD = "15/11/2023", Status = "Hết hạn", Class = "danger" },
                new { Ten = "Berberin 100mg", SoLo = "LOT-2210C", HSD = "02/12/2023", Status = "Sắp hết hạn", Class = "warning" },
                new { Ten = "Nước muối sinh lý", SoLo = "LOT-2305B", HSD = "10/12/2023", Status = "Sắp hết hạn", Class = "warning" }
            };

            // 4. Phiếu nhập kho gần đây
            ViewBag.DsPhieuNhap = new List<dynamic> {
                new { Ma = "PN-20231024-01", Ngay = "24/10/2023 09:30", NCC = "Công ty Dược phẩm Trung ương 1", Tong = "15,450,000" },
                new { Ma = "PN-20231023-02", Ngay = "23/10/2023 14:15", NCC = "Công ty TNHH Dược phẩm Hậu Giang", Tong = "8,200,000" },
                new { Ma = "PN-20231021-01", Ngay = "21/10/2023 10:00", NCC = "Công ty CP Traphaco", Tong = "24,100,000" }
            };

            /* [NOTE SQL]:
               ViewBag.SapHetHang = _context.SanPhams.Count(s => s.SoLuongTon <= s.MucCanhBao);
               ViewBag.DsHetHan = _context.VwKiemTraHanDungs.Where(l => l.SoNgayConLai <= 30).ToList();
            */

            return View();
=======
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
>>>>>>> a1acad9 (Hoan thien giao dien tong quan kho)
        }
    }
}