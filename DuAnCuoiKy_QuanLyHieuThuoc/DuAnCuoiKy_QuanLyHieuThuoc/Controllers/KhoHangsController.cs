using Microsoft.AspNetCore.Mvc;
using DuAnCuoiKy_QuanLyHieuThuoc.Models;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Controllers
{
    // ============================================
    // CONTROLLER XEM TỒN KHO
    // ============================================
    public class KhoHangsController : Controller
    {
        private const int PAGE_SIZE = 5;

        public IActionResult Index(string keyword = "", string loai = "", int page = 1)
        {
            var allProducts = GetFakeProducts();

            // 1. LỌC THEO TỪ KHÓA
            if (!string.IsNullOrWhiteSpace(keyword))
                allProducts = allProducts
                    .Where(x => x.TenThuoc.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                    .ToList();

            // 2. LỌC THEO LOẠI
            if (!string.IsNullOrWhiteSpace(loai))
                allProducts = allProducts
                    .Where(x => x.Loai == loai)
                    .ToList();

            // 3. PHÂN TRANG
            int totalItems = allProducts.Count;
            int totalPages = (int)Math.Ceiling(totalItems / (double)PAGE_SIZE);
            page = Math.Max(1, Math.Min(page, Math.Max(1, totalPages)));

            var paged = allProducts
                .Skip((page - 1) * PAGE_SIZE)
                .Take(PAGE_SIZE)
                .ToList();

            // 4. DANH MỤC LỌC
            var categories = GetFakeProducts()
                .Select(x => x.Loai)
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            ViewBag.Keyword = keyword;
            ViewBag.SelectedLoai = loai;
            ViewBag.Page = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalItems = totalItems;
            ViewBag.PageSize = PAGE_SIZE;
            ViewBag.Categories = categories;

            return View(paged);
        }

        // ── DỮ LIỆU GIẢ ─────────────────────────────────────
        private List<SanPhamKho> GetFakeProducts()
        {
            return new List<SanPhamKho>
            {
                new() { TenThuoc = "Panadol Extra 500mg",      Loai = "Thuốc giảm đau",  DonViTinh = "Hộp", TonKho = 1250, TrangThai = "San sang"    },
                new() { TenThuoc = "Amoxicillin 250mg",        Loai = "Kháng sinh",       DonViTinh = "Vỉ",  TonKho = 45,   TrangThai = "Binh thuong" },
                new() { TenThuoc = "Vitamin C 1000mg",         Loai = "Thực phẩm CN",     DonViTinh = "Lọ",  TonKho = 12,   TrangThai = "Sap het"     },
                new() { TenThuoc = "Khẩu trang y tế 4 lớp",   Loai = "Vật tư y tế",      DonViTinh = "Hộp", TonKho = 0,    TrangThai = "Het hang"    },
                new() { TenThuoc = "Omeprazole 20mg",          Loai = "Tiêu hóa",         DonViTinh = "Viên",TonKho = 890,  TrangThai = "San sang"    },
                new() { TenThuoc = "Augmentin 1g",             Loai = "Kháng sinh",       DonViTinh = "Hộp", TonKho = 20,   TrangThai = "Sap het"     },
                new() { TenThuoc = "Ibuprofen 400mg",          Loai = "Thuốc giảm đau",   DonViTinh = "Viên",TonKho = 340,  TrangThai = "San sang"    },
                new() { TenThuoc = "Cetirizine 10mg",          Loai = "Dị ứng",           DonViTinh = "Vỉ",  TonKho = 78,   TrangThai = "Binh thuong" },
                new() { TenThuoc = "Metformin 500mg",          Loai = "Tiểu đường",       DonViTinh = "Viên",TonKho = 0,    TrangThai = "Het hang"    },
                new() { TenThuoc = "Atorvastatin 20mg",        Loai = "Tim mạch",         DonViTinh = "Viên",TonKho = 155,  TrangThai = "Binh thuong" },
                new() { TenThuoc = "Losartan 50mg",            Loai = "Tim mạch",         DonViTinh = "Viên",TonKho = 210,  TrangThai = "San sang"    },
                new() { TenThuoc = "Betadine 10% 30ml",        Loai = "Vật tư y tế",      DonViTinh = "Chai",TonKho = 8,    TrangThai = "Sap het"     },
                new() { TenThuoc = "Smecta 3g",                Loai = "Tiêu hóa",         DonViTinh = "Gói", TonKho = 430,  TrangThai = "San sang"    },
                new() { TenThuoc = "Nexium 40mg",              Loai = "Tiêu hóa",         DonViTinh = "Viên",TonKho = 0,    TrangThai = "Het hang"    },
                new() { TenThuoc = "Glucosamine 500mg",        Loai = "Thực phẩm CN",     DonViTinh = "Hộp", TonKho = 62,   TrangThai = "Binh thuong" },
            };
        }
    }
}