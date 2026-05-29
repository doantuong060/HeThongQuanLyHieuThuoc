using Microsoft.AspNetCore.Mvc;
using DuAnCuoiKy_QuanLyHieuThuoc.Models;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Controllers
{
    public class TaiKhoansController : Controller
    {
        // GET: TaiKhoans/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: TaiKhoans/Login
        [HttpPost]
        public IActionResult Login(string TenDangNhap, string MatKhau)
        {
            // --- [VỊ TRÍ HARDCODE HỆ THỐNG] ---
            // Giả lập tài khoản admin để test giao diện
            if (TenDangNhap == "admin" && MatKhau == "admin")
            {
                // Sau này tại đây sẽ gọi LINQ: 
                // var user = _context.TaiKhoans.FirstOrDefault(u => u.TenDangNhap == TenDangNhap && u.MatKhau == MatKhau);

                return RedirectToAction("Index_Admin", "Home");
            }
            else if (TenDangNhap == "nhanvienkho" && MatKhau == "nhanvienkho")
            {
                return RedirectToAction("Index_Admin", "Home");
            }
            else if (TenDangNhap == "nhanvienquay" && MatKhau == "nhanvienquay")
            {
                return RedirectToAction("Index_Admin", "Home");
            }
            else
            {
                ViewBag.Error = "Tài khoản hoặc mật khẩu không đúng!";
                return View();
            }
        }

        public IActionResult Logout()
        {
            return RedirectToAction("Login");
        }
    }
}