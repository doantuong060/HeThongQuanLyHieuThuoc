using DuAnCuoiKy_QuanLyHieuThuoc.Business;
using DuAnCuoiKy_QuanLyHieuThuoc.Enums;
using DuAnCuoiKy_QuanLyHieuThuoc.Extensions;
using DuAnCuoiKy_QuanLyHieuThuoc.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Controllers
{
    public class BanHangsController : Controller
    {
        private const string CART_KEY = "CART";
        private readonly BanHangsBusiness _business;

        // Khởi tạo Business thông qua DI Context
        public BanHangsController(HieuThuocDbContext context)
        {
            _business = new BanHangsBusiness(context);
        }

        // ============================================
        // 1. TRANG POS (ĐỔ DỮ LIỆU THẬT)
        // ============================================
        public IActionResult Index(string keyword = "", string loai = "")
        {
            // Kéo dữ liệu từ SQL thông qua lớp Business
            var products = _business.GetDanhSachSanPham(keyword, loai);

            // Xử lý giỏ hàng
            var cart = GetCart();
            decimal tongTien = cart.Sum(x => x.ThanhTien);

            ViewBag.Keyword = keyword;
            ViewBag.SelectedLoai = loai;
            ViewBag.Cart = cart;
            ViewBag.TongTien = tongTien;

            return View(products);
        }

        // ============================================
        // 2. THÊM VÀO GIỎ HÀNG
        // ============================================
        public IActionResult AddToCart(string id, string keyword = "", string loai = "")
        {
            var products = _business.GetDanhSachSanPham("", "");
            var sp = products.FirstOrDefault(x => x.MaSp == id);

            if (sp == null) return NotFound();

            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.MaSP == id);

            // Check realtime tồn kho DB
            int realTonKho = _business.GetTonKhoThucTe(id);

            if (item == null)
            {
                if (realTonKho > 0)
                {
                    cart.Add(new CartItem
                    {
                        MaSP = sp.MaSp,
                        TenSP = sp.TenSp,
                        GiaBan = sp.GiaBan,
                        SoLuong = 1,
                        TonKho = realTonKho
                    });
                }
                else
                {
                    TempData["Error"] = "Sản phẩm đã hết hàng trong kho!";
                }
            }
            else
            {
                if (item.SoLuong < realTonKho)
                {
                    item.SoLuong++;
                }
                else
                {
                    TempData["Error"] = $"'{item.TenSP}' đã đạt tối đa tồn kho ({realTonKho})!";
                }
            }

            SaveCart(cart);
            return RedirectToAction("Index", new { keyword = keyword, loai = loai });
        }

        // ============================================
        // 3. TĂNG / GIẢM / XÓA SẢN PHẨM GIỎ HÀNG
        // ============================================
        public IActionResult IncreaseQuantity(string id, string keyword = "", string loai = "")
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.MaSP == id);

            if (item != null)
            {
                int realTonKho = _business.GetTonKhoThucTe(id);
                if (item.SoLuong < realTonKho) item.SoLuong++;
                else TempData["Error"] = $"Kho chỉ còn {realTonKho} sản phẩm!";
            }

            SaveCart(cart);
            return RedirectToAction("Index", new { keyword = keyword, loai = loai });
        }

        public IActionResult DecreaseQuantity(string id, string keyword = "", string loai = "")
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.MaSP == id);

            if (item != null)
            {
                item.SoLuong--;
                if (item.SoLuong <= 0) cart.Remove(item);
            }

            SaveCart(cart);
            return RedirectToAction("Index", new { keyword = keyword, loai = loai });
        }

        public IActionResult RemoveItem(string id, string keyword = "", string loai = "")
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.MaSP == id);
            if (item != null) cart.Remove(item);

            SaveCart(cart);
            return RedirectToAction("Index", new { keyword = keyword, loai = loai });
        }

        public IActionResult ClearCart(string keyword = "", string loai = "")
        {
            HttpContext.Session.Remove(CART_KEY);
            TempData["Success"] = "Đã hủy bill hiện tại!";
            return RedirectToAction("Index", new { keyword = keyword, loai = loai });
        }

        // ============================================
        // 4. THANH TOÁN (GỌI XUỐNG DB & HỨNG ENUM)
        // ============================================
        [HttpPost]
        public IActionResult Checkout(PTThanhToan phuongThuc = PTThanhToan.TienMat)
        {
            var cart = GetCart();

            if (cart.Count == 0)
            {
                TempData["Error"] = "Giỏ hàng đang trống!";
                return RedirectToAction("Index");
            }

            string maNV = User.Identity?.IsAuthenticated == true ? User.GetMaNV() : "NV0002";
            string ghiChuThanhToan = $"Thanh toán bằng: {phuongThuc}"; // Map Enum ra chuỗi

            try
            {
                string maHD = _business.ThanhToanDonHang(cart, maNV, ghiChuThanhToan);

                HttpContext.Session.Remove(CART_KEY);
                TempData["Success"] = $"Thanh toán thành công! Mã HĐ: {maHD}";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi thanh toán: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        // ============================================
        // HELPER SESSIONS
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

        private void SaveCart(List<CartItem> cart)
        {
            HttpContext.Session.SetString(CART_KEY, JsonConvert.SerializeObject(cart));
        }
    }
}