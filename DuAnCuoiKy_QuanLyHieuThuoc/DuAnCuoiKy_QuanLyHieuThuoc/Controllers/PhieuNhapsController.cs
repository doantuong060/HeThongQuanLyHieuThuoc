using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using DuAnCuoiKy_QuanLyHieuThuoc.Models;
using DuAnCuoiKy_QuanLyHieuThuoc.Models.ViewModels;

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

        public IActionResult Index()
        {
            var dsPhieu = new List<dynamic>
            {
                new {
                    Ma = "PN-20231024-01",
                    Ngay = "24/10/2023 09:30",
                    NCC = "Công ty Dược phẩm Trung ương I",
                    SoMatHang = 15,
                    TongTien = "125,400,000",
                    Status = "Đã nhập kho",
                    Class = "success"
                },

                new {
                    Ma = "PN-20231024-02",
                    Ngay = "24/10/2023 14:15",
                    NCC = "Nhà phân phối Thuốc Việt",
                    SoMatHang = 8,
                    TongTien = "45,200,000",
                    Status = "Chờ duyệt",
                    Class = "warning"
                },

                new {
                    Ma = "PN-20231023-01",
                    Ngay = "23/10/2023 10:00",
                    NCC = "Dược Hậu Giang",
                    SoMatHang = 42,
                    TongTien = "310,500,000",
                    Status = "Đã nhập kho",
                    Class = "success"
                },

                new {
                    Ma = "PN-20231022-03",
                    Ngay = "22/10/2023 16:45",
                    NCC = "Công ty TNHH Dược phẩm Đông Á",
                    SoMatHang = 5,
                    TongTien = "12,000,000",
                    Status = "Đã hủy",
                    Class = "danger"
                },

                new {
                    Ma = "PN-20231021-01",
                    Ngay = "21/10/2023 08:15",
                    NCC = "Traphaco",
                    SoMatHang = 20,
                    TongTien = "89,600,000",
                    Status = "Đã nhập kho",
                    Class = "success"
                }
            };

            ViewBag.DsPhieuNhap = dsPhieu;

            ViewBag.NhaCungCap = new SelectList(new[]
            {
                "Tất cả NCC",
                "Dược phẩm TW1",
                "Dược Hậu Giang",
                "Traphaco"
            });

            ViewBag.TrangThai = new SelectList(new[]
            {
                "Tất cả trạng thái",
                "Đã nhập kho",
                "Chờ duyệt",
                "Đã hủy"
            });

            return View();
        }

        // =====================================================
        // GET CREATE
        // =====================================================

        [HttpGet]
        public IActionResult Create()
        {
            var model = new PhieuNhapCreateViewModel();

            // NCC
            model.DsNhaCungCap = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = "NCC001",
                    Text = "Dược phẩm Trung ương 1"
                },

                new SelectListItem
                {
                    Value = "NCC002",
                    Text = "Dược Hậu Giang"
                },

                new SelectListItem
                {
                    Value = "NCC003",
                    Text = "Traphaco"
                }
            };

            // SẢN PHẨM
            model.DsSanPham = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = "TH001",
                    Text = "Amoxicillin 500mg"
                },

                new SelectListItem
                {
                    Value = "TH002",
                    Text = "Paracetamol 500mg"
                },

                new SelectListItem
                {
                    Value = "TH003",
                    Text = "Vitamin C 1000mg"
                }
            };

            // TẠO 3 DÒNG NHẬP SẴN
            model.DanhSachLoHang = new List<LoHangNhapVM>
            {
                new LoHangNhapVM
                {
                    HanSuDung = DateOnly.FromDateTime(DateTime.Now.AddMonths(12))
                },

                new LoHangNhapVM
                {
                    HanSuDung = DateOnly.FromDateTime(DateTime.Now.AddMonths(12))
                },

                new LoHangNhapVM
                {
                    HanSuDung = DateOnly.FromDateTime(DateTime.Now.AddMonths(12))
                }
            };

            return View(model);
        }

        // =====================================================
        // POST CREATE
        // =====================================================

        [HttpPost]
        public IActionResult Create(PhieuNhapCreateViewModel model)
        {
            // VALIDATION HSD
            foreach (var lo in model.DanhSachLoHang)
            {
                if (lo.HanSuDung <= DateOnly.FromDateTime(DateTime.Now))
                {
                    ModelState.AddModelError(
                        "",
                        $"Lô {lo.SoLo}: Hạn sử dụng phải lớn hơn ngày hiện tại."
                    );
                }

                if (lo.GiaNhap <= 0)
                {
                    ModelState.AddModelError(
                        "",
                        $"Lô {lo.SoLo}: Giá nhập phải > 0."
                    );
                }
            }

            // LOAD LẠI DROPDOWN NẾU LỖI
            if (!ModelState.IsValid)
            {
                model.DsNhaCungCap = new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Value = "NCC001",
                        Text = "Dược phẩm Trung ương 1"
                    },

                    new SelectListItem
                    {
                        Value = "NCC002",
                        Text = "Dược Hậu Giang"
                    },

                    new SelectListItem
                    {
                        Value = "NCC003",
                        Text = "Traphaco"
                    }
                };

                model.DsSanPham = new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Value = "TH001",
                        Text = "Amoxicillin 500mg"
                    },

                    new SelectListItem
                    {
                        Value = "TH002",
                        Text = "Paracetamol 500mg"
                    },

                    new SelectListItem
                    {
                        Value = "TH003",
                        Text = "Vitamin C 1000mg"
                    }
                };

                return View(model);
            }

            // =================================================
            // HARDCODE DEMO
            // =================================================

            TempData["Success"] =
                "Tạo phiếu nhập thành công!";

            return RedirectToAction(nameof(Create));

            /*
            // SQL THỰC TẾ

            var phieuNhap = new PhieuNhap
            {
                MaPhieuNhap = "PN001",
                MaNcc = model.MaNcc,
                MaNv = "NV001",
                NgayNhap = DateTime.Now
            };

            _context.PhieuNhaps.Add(phieuNhap);
            _context.SaveChanges();

            foreach (var item in model.DanhSachLoHang)
            {
                var loHang = new LoHang
                {
                    SoLo = item.SoLo,
                    MaSp = item.MaSp,
                    MaPhieuNhap = phieuNhap.MaPhieuNhap,
                    GiaNhap = item.GiaNhap,
                    HanSuDung = item.HanSuDung,
                    SoLuongNhap = item.SoLuongNhap,
                    SoLuongConLai = item.SoLuongNhap
                };

                _context.LoHangs.Add(loHang);
            }

            _context.SaveChanges();
            */
        }
    }
}