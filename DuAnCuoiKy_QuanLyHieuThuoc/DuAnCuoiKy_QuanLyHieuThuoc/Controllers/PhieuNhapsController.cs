using DuAnCuoiKy_QuanLyHieuThuoc.Models;
using DuAnCuoiKy_QuanLyHieuThuoc.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Controllers
{
    public class PhieuNhapsController : Controller
    {
        private readonly HieuThuocDbContext _context;

        public PhieuNhapsController(HieuThuocDbContext context)
        {
            _context = context;
        }

        // =====================================================
        // DANH SÁCH PHIẾU NHẬP
        // =====================================================
        public IActionResult Index(
            string? maNcc,
            DateTime? tuNgay,
            DateTime? denNgay)
        {
            var query = _context.PhieuNhaps
                .Include(x => x.MaNccNavigation)
                .Include(x => x.LoHangs)
                .AsQueryable();

            if (!string.IsNullOrEmpty(maNcc))
            {
                query = query.Where(x => x.MaNcc == maNcc);
            }

            if (tuNgay.HasValue)
            {
                query = query.Where(x => x.NgayNhap >= tuNgay.Value);
            }

            if (denNgay.HasValue)
            {
                query = query.Where(x => x.NgayNhap <= denNgay.Value);
            }

            var dsPhieu = query
                .OrderByDescending(x => x.NgayNhap)
                .Select(x => new
                {
                    MaPhieu = x.MaPhieuNhap,
                    NgayNhap = x.NgayNhap,
                    TenNcc = x.MaNccNavigation.TenNcc,
                    SoMatHang = x.LoHangs.Count,
                    TongTien = x.TongTien ?? 0
                })
                .ToList();

            ViewBag.DsPhieuNhap = dsPhieu;

            ViewBag.DsNCC = new SelectList(
                _context.NhaCungCaps.ToList(),
                "MaNcc",
                "TenNcc",
                maNcc);

            ViewBag.TuNgay = tuNgay?.ToString("yyyy-MM-dd");
            ViewBag.DenNgay = denNgay?.ToString("yyyy-MM-dd");

            return View();
        }

        // =====================================================
        // GET CREATE
        // =====================================================
        [HttpGet]
        public IActionResult Create()
        {
            var vm = TaoViewModel();

            vm.DanhSachLoHang.Add(new LoHangNhapVM
            {
                HanSuDung = DateOnly.FromDateTime(
                    DateTime.Now.AddMonths(6))
            });

            return View(vm);
        }

        // =====================================================
        // POST CREATE
        // =====================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PhieuNhapCreateViewModel vm)
        {
            vm = NapLaiCombobox(vm);

            if (vm.DanhSachLoHang == null ||
                vm.DanhSachLoHang.Count == 0)
            {
                ModelState.AddModelError("",
                    "Phải nhập ít nhất 1 mặt hàng.");
            }

            foreach (var item in vm.DanhSachLoHang)
            {
                if (item.HanSuDung <=
                    DateOnly.FromDateTime(DateTime.Now))
                {
                    ModelState.AddModelError("",
                        $"Lô {item.SoLo}: Hạn sử dụng phải lớn hơn ngày hiện tại.");
                }

                if (item.GiaNhap <= 0)
                {
                    ModelState.AddModelError("",
                        $"Lô {item.SoLo}: Giá nhập phải lớn hơn 0.");
                }

                if (item.SoLuongNhap <= 0)
                {
                    ModelState.AddModelError("",
                        $"Lô {item.SoLo}: Số lượng phải lớn hơn 0.");
                }
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            // Lấy phiếu nhập lớn nhất hiện tại
var maCuoi = _context.PhieuNhaps
    .OrderByDescending(x => x.MaPhieuNhap)
    .Select(x => x.MaPhieuNhap)
    .FirstOrDefault();

int soMoi = 1;

if (!string.IsNullOrEmpty(maCuoi))
{
    soMoi = int.Parse(maCuoi.Substring(2)) + 1;
}

string maPhieu = $"PN{soMoi:0000}";

            decimal tongTien = vm.DanhSachLoHang
                .Sum(x => x.GiaNhap * x.SoLuongNhap);

            // ===== LAY NHAN VIEN DA TON TAI =====
            string? maNv = _context.NhanViens
                .Select(x => x.MaNv)
                .FirstOrDefault();

            if (string.IsNullOrEmpty(maNv))
            {
                ModelState.AddModelError("",
                    "Không tìm thấy nhân viên trong hệ thống.");

                return View(vm);
            }

            var phieuNhap = new PhieuNhap
            {
                MaPhieuNhap = maPhieu,
                NgayNhap = DateTime.Now,
                MaNcc = vm.MaNcc,
                MaNv = maNv,
                TongTien = tongTien,
                GhiChu = ""
            };

            _context.PhieuNhaps.Add(phieuNhap);

            foreach (var item in vm.DanhSachLoHang)
            {
                var lo = new LoHang
                {
                    SoLo = item.SoLo,
                    MaSp = item.MaSp,
                    MaPhieuNhap = maPhieu,
                    GiaNhap = item.GiaNhap,
                    HanSuDung = item.HanSuDung,
                    SoLuongNhap = item.SoLuongNhap,
                    SoLuongConLai = item.SoLuongNhap
                };

                _context.LoHangs.Add(lo);
            }

            _context.SaveChanges();

            TempData["Success"] =
                "Tạo phiếu nhập thành công.";

            return RedirectToAction(nameof(Index));
        }

        // =====================================================
        // CHI TIẾT PHIẾU NHẬP
        // =====================================================
        public IActionResult Details(string id)
        {
            var phieu = _context.PhieuNhaps
                .Include(x => x.MaNccNavigation)
                .Include(x => x.LoHangs)
                    .ThenInclude(x => x.MaSpNavigation)
                .FirstOrDefault(x => x.MaPhieuNhap == id);

            if (phieu == null)
            {
                return NotFound();
            }

            return View(phieu);
        }

        // =====================================================
        // HELPER
        // =====================================================
        private PhieuNhapCreateViewModel TaoViewModel()
        {
            return new PhieuNhapCreateViewModel
            {
                DsNhaCungCap = _context.NhaCungCaps
                    .Select(x => new SelectListItem
                    {
                        Value = x.MaNcc,
                        Text = x.TenNcc
                    })
                    .ToList(),

                DsSanPham = _context.SanPhams
                    .Select(x => new SelectListItem
                    {
                        Value = x.MaSp,
                        Text = x.TenSp
                    })
                    .ToList()
            };
        }

        private PhieuNhapCreateViewModel NapLaiCombobox(
            PhieuNhapCreateViewModel vm)
        {
            vm.DsNhaCungCap = _context.NhaCungCaps
                .Select(x => new SelectListItem
                {
                    Value = x.MaNcc,
                    Text = x.TenNcc
                })
                .ToList();

            vm.DsSanPham = _context.SanPhams
                .Select(x => new SelectListItem
                {
                    Value = x.MaSp,
                    Text = x.TenSp
                })
                .ToList();

            return vm;
        }
    }
}