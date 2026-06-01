using DuAnCuoiKy_QuanLyHieuThuoc.Helpers;
using DuAnCuoiKy_QuanLyHieuThuoc.Models;
using DuAnCuoiKy_QuanLyHieuThuoc.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
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
            string? keyword,
            DateTime? tuNgay,
            DateTime? denNgay)
        {
            var query = _context.PhieuNhaps
                .Include(x => x.MaNccNavigation)
                .Include(x => x.LoHangs)
                .AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.MaPhieuNhap.Contains(keyword));

            if (tuNgay.HasValue)
                query = query.Where(x => x.NgayNhap >= tuNgay.Value);

            if (denNgay.HasValue)
                query = query.Where(x => x.NgayNhap <= denNgay.Value.AddDays(1));

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

            return View();
        }

        // =====================================================
        // GET CREATE
        // =====================================================
        [HttpGet]
        public IActionResult Create()
        {
            var vm = PhieuNhapHelper.TaoViewModel(_context);

            vm.DanhSachLoHang.Add(new LoHangNhapVM
            {
                HanSuDung = DateOnly.FromDateTime(DateTime.Now.AddMonths(6))
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
            vm = PhieuNhapHelper.NapLaiCombobox(_context, vm);

            if (vm.DanhSachLoHang == null || vm.DanhSachLoHang.Count == 0)
            {
                ModelState.AddModelError("", "Phải nhập ít nhất 1 mặt hàng.");
                return View(vm);
            }

            // 1. KIỂM TRA TRÙNG SỐ LÔ NGAY TRONG FORM (Người dùng nhập trùng 2 dòng trên giao diện)
            var cacSoLoBiTrungTrongForm = vm.DanhSachLoHang
                .GroupBy(x => x.SoLo)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (cacSoLoBiTrungTrongForm.Any())
            {
                ModelState.AddModelError("", $"Số lô không được trùng nhau trong cùng một phiếu nhập: {string.Join(", ", cacSoLoBiTrungTrongForm)}");
            }

            // Kiểm tra các điều kiện logic khác của từng lô hàng
            foreach (var item in vm.DanhSachLoHang)
            {
                if (item.HanSuDung <= DateOnly.FromDateTime(DateTime.Now))
                    ModelState.AddModelError("",
                        $"Lô {item.SoLo}: Hạn sử dụng phải lớn hơn ngày hiện tại.");

                if (item.GiaNhap <= 0)
                    ModelState.AddModelError("",
                        $"Lô {item.SoLo}: Giá nhập phải lớn hơn 0.");

                if (item.SoLuongNhap <= 0)
                    ModelState.AddModelError("",
                        $"Lô {item.SoLo}: Số lượng phải lớn hơn 0.");

                // 2. KIỂM TRA TRÙNG SỐ LÔ VỚI DATABASE (Số lô này đã từng được nhập trước đây)
                bool daTonTaiTrongDb = _context.LoHangs.Any(x => x.SoLo == item.SoLo);
                if (daTonTaiTrongDb)
                {
                    ModelState.AddModelError("", $"Số lô '{item.SoLo}' đã tồn tại trong hệ thống. Vui lòng đặt ký hiệu Số lô khác.");
                }
            }

            // Nếu có bất kỳ lỗi nào ở trên, trả về View hiển thị danh sách lỗi cho người dùng
            if (!ModelState.IsValid)
                return View(vm);

            string maPhieu = PhieuNhapHelper.SinhMaPhieu(_context);

            decimal tongTien = vm.DanhSachLoHang
                .Sum(x => x.GiaNhap * x.SoLuongNhap);

            string? maNv = _context.NhanViens
                .Select(x => x.MaNv)
                .FirstOrDefault();

            if (string.IsNullOrEmpty(maNv))
            {
                ModelState.AddModelError("", "Không tìm thấy nhân viên trong hệ thống.");
                return View(vm);
            }

            // Dùng Transaction để đảm bảo an toàn dữ liệu khi lưu nhiều bảng
            using var transaction = _context.Database.BeginTransactionAsync();
            try
            {
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
                    _context.LoHangs.Add(new LoHang
                    {
                        SoLo = item.SoLo,
                        MaSp = item.MaSp,
                        MaPhieuNhap = maPhieu,
                        GiaNhap = item.GiaNhap,
                        HanSuDung = item.HanSuDung,
                        SoLuongNhap = item.SoLuongNhap,
                        SoLuongConLai = item.SoLuongNhap
                    });
                }

                _context.SaveChanges();
                _context.Database.CommitTransaction(); // Hoàn tất lưu dữ liệu an toàn
            }
            catch (Exception ex)
            {
                _context.Database.RollbackTransaction(); // Hủy bỏ nếu có lỗi phát sinh đột xuất
                ModelState.AddModelError("", "Lỗi hệ thống khi lưu phiếu nhập: " + (ex.InnerException?.Message ?? ex.Message));
                return View(vm);
            }

            TempData["Success"] = "Tạo phiếu nhập thành công.";
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
                return NotFound();

            return View(phieu);
        }

        // =====================================================
        // TỒN KHO & CẢNH BÁO
        // =====================================================
        public IActionResult TonKho()
        {
            var dsTonKho = _context.LoHangs
                .Include(x => x.MaSpNavigation)
                .ToList()
                .GroupBy(x => x.MaSp)
                .Select(g =>
                {
                    var sp = g.First().MaSpNavigation;
                    int ton = g.Sum(x => x.SoLuongConLai);

                    var loGanHetHan = g.OrderBy(x => x.HanSuDung).FirstOrDefault();

                    string status, css;

                    if (ton <= sp.MucCanhBao)
                    {
                        status = "Sắp hết";
                        css = "warning";
                    }
                    else
                    {
                        status = "An toàn";
                        css = "success";
                    }

                    return new
                    {
                        Ma = sp.MaSp,
                        Ten = sp.TenSp,
                        Loai = sp.LoaiSp,
                        DVT = sp.MaDvt,
                        Ton = ton,
                        Nguong = sp.MucCanhBao,
                        Progress = Math.Min(
                            (int)((double)ton / Math.Max(sp.MucCanhBao, 1) * 100), 100),
                        Class = css,
                        Status = status,
                        LoGanHSD = loGanHetHan != null
                            ? $"{loGanHetHan.SoLo} ({loGanHetHan.HanSuDung:dd/MM/yyyy})"
                            : ""
                    };
                })
                .ToList();

            ViewBag.DsTonKho = dsTonKho;
            ViewBag.TongMaThuoc = dsTonKho.Count;
            ViewBag.AnToanCount = dsTonKho.Count(x => x.Class == "success");
            ViewBag.SapHetHangCount = dsTonKho.Count(x => x.Class == "warning");
            ViewBag.NguyCapCount = dsTonKho.Count(x => x.Class == "danger");

            return View();
        }
    }
}