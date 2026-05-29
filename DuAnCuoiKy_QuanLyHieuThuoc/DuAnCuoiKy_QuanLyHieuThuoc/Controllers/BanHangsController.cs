using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using DuAnCuoiKy_QuanLyHieuThuoc.Models;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Controllers
{
    // ============================================
    // CONTROLLER BÁN HÀNG
    // ============================================
    public class BanHangsController : Controller
    {
        private const string CART_KEY = "CART";

        // ============================================
        // TRANG POS
        // ============================================
        public IActionResult Index(string keyword = "", string loai = "")
        {
            var products = GetFakeProducts();

            // 1. LỌC THEO TỪ KHÓA
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                products = products
                    .Where(x => x.TenSP.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // 2. LỌC THEO DANH MỤC
            if (!string.IsNullOrWhiteSpace(loai))
            {
                products = products
                    .Where(x => x.TenLoai == loai)
                    .ToList();
            }

            // GIỎ HÀNG & TỔNG TIỀN
            var cart = GetCart();
            decimal tongTien = cart.Sum(x => x.ThanhTien);

            // 3. TRẢ DỮ LIỆU VỀ VIEW
            ViewBag.Keyword = keyword;
            ViewBag.SelectedLoai = loai;

            ViewBag.Cart = cart;
            ViewBag.TongTien = tongTien;

            return View(products);
        }

        // ============================================
        // THÊM VÀO GIỎ
        // ============================================
        public IActionResult AddToCart(string id, string keyword = "", string loai = "")
        {
            var sp = GetFakeProducts().FirstOrDefault(x => x.MaSP == id);

            if (sp == null)
            {
                return NotFound();
            }

            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.MaSP == id);

            // CHƯA CÓ TRONG GIỎ
            if (item == null)
            {
                cart.Add(new CartItem
                {
                    MaSP = sp.MaSP,
                    TenSP = sp.TenSP,
                    GiaBan = sp.GiaBan,
                    SoLuong = 1,
                    TonKho = sp.TonKho
                });
            }
            else
            {
                // KIỂM TRA TỒN KHO
                if (item.SoLuong < item.TonKho)
                {
                    item.SoLuong++;
                }
                else
                {
                    TempData["Error"] = $"'{item.TenSP}' đã đạt tối đa tồn kho!";
                }
            }

            SaveCart(cart);

            // Quay về Index kèm theo trạng thái tìm kiếm/lọc cũ
            return RedirectToAction("Index", new { keyword = keyword, loai = loai });
        }

        // ============================================
        // TĂNG SỐ LƯỢNG
        // ============================================
        public IActionResult IncreaseQuantity(string id, string keyword = "", string loai = "")
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.MaSP == id);

            if (item != null)
            {
                if (item.SoLuong < item.TonKho)
                {
                    item.SoLuong++;
                }
                else
                {
                    TempData["Error"] = $"'{item.TenSP}' đã đạt tối đa tồn kho!";
                }
            }

            SaveCart(cart);

            return RedirectToAction("Index", new { keyword = keyword, loai = loai });
        }

        // ============================================
        // GIẢM SỐ LƯỢNG
        // ============================================
        public IActionResult DecreaseQuantity(string id, string keyword = "", string loai = "")
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.MaSP == id);

            if (item != null)
            {
                item.SoLuong--;

                if (item.SoLuong <= 0)
                {
                    cart.Remove(item);
                }
            }

            SaveCart(cart);

            return RedirectToAction("Index", new { keyword = keyword, loai = loai });
        }

        // ============================================
        // XÓA SẢN PHẨM
        // ============================================
        public IActionResult RemoveItem(string id, string keyword = "", string loai = "")
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.MaSP == id);

            if (item != null)
            {
                cart.Remove(item);
            }

            SaveCart(cart);

            return RedirectToAction("Index", new { keyword = keyword, loai = loai });
        }

        // ============================================
        // XÓA TOÀN BỘ GIỎ HÀNG
        // ============================================
        public IActionResult ClearCart(string keyword = "", string loai = "")
        {
            HttpContext.Session.Remove(CART_KEY);

            TempData["Success"] = "Đã xóa toàn bộ giỏ hàng!";

            return RedirectToAction("Index", new { keyword = keyword, loai = loai });
        }

        // ============================================
        // THANH TOÁN DEMO
        // ============================================
        [HttpPost]
        public IActionResult Checkout()
        {
            var cart = GetCart();

            if (cart.Count == 0)
            {
                TempData["Error"] = "Giỏ hàng đang trống!";
                return RedirectToAction("Index");
            }

            decimal tongTien = cart.Sum(x => x.ThanhTien);
            string maHD = "HD" + DateTime.Now.ToString("yyyyMMddHHmmss");

            // SAU NÀY PHASE 3: INSERT SQL TẠI ĐÂY

            HttpContext.Session.Remove(CART_KEY);

            TempData["Success"] = $"Thanh toán thành công - Mã HD: {maHD} - Tổng tiền: {tongTien:N0} VNĐ";

            return RedirectToAction("Index");
        }

        // ============================================
        // LẤY GIỎ HÀNG KHỎI SESSION
        // ============================================
        private List<CartItem> GetCart()
        {
            var session = HttpContext.Session.GetString(CART_KEY);

            if (!string.IsNullOrEmpty(session))
            {
                return JsonConvert.DeserializeObject<List<CartItem>>(session) ?? new List<CartItem>();
            }

            return new List<CartItem>();
        }

        // ============================================
        // LƯU GIỎ HÀNG VÀO SESSION
        // ============================================
        private void SaveCart(List<CartItem> cart)
        {
            HttpContext.Session.SetString(CART_KEY, JsonConvert.SerializeObject(cart));
        }

        // ============================================
        // DỮ LIỆU GIẢ (MOCK DATA)
        // ============================================
        private List<SanPhamDemo> GetFakeProducts()
        {
            return new List<SanPhamDemo>
            {
                new SanPhamDemo
                {
                    MaSP = "SP001",
                    TenSP = "Panadol Extra",
                    MoTa = "Hộp 15 vỉ x 10 viên",
                    GiaBan = 185000,
                    TonKho = 120,
                    TenLoai = "Giảm đau",
                    CanToa = false,
                    DonVi = "Hộp"
                },
                new SanPhamDemo
                {
                    MaSP = "SP002",
                    TenSP = "Amoxicillin 500mg",
                    MoTa = "Hộp 10 vỉ x 10 viên",
                    GiaBan = 120000,
                    TonKho = 45,
                    TenLoai = "Kháng sinh",
                    CanToa = true,
                    DonVi = "Hộp"
                },
                new SanPhamDemo
                {
                    MaSP = "SP003",
                    TenSP = "Vitamin C 1000mg",
                    MoTa = "Tuýp 10 viên sủi",
                    GiaBan = 45000,
                    TonKho = 80,
                    TenLoai = "Vitamin",
                    CanToa = false,
                    DonVi = "Tuýp"
                },
                new SanPhamDemo
                {
                    MaSP = "SP004",
                    TenSP = "Omeprazole 20mg",
                    MoTa = "Hộp 3 vỉ x 10 viên",
                    GiaBan = 95000,
                    TonKho = 50,
                    TenLoai = "Tiêu hóa",
                    CanToa = false,
                    DonVi = "Hộp"
                },
                new SanPhamDemo
                {
                    MaSP = "SP005",
                    TenSP = "Augmentin 1g",
                    MoTa = "Hộp 2 vỉ x 7 viên",
                    GiaBan = 250000,
                    TonKho = 20,
                    TenLoai = "Kháng sinh",
                    CanToa = true,
                    DonVi = "Hộp"
                }
            };
        }
    }
}