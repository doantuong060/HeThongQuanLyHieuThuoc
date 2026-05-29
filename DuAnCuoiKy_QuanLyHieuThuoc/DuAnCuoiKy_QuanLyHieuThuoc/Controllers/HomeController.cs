using DuAnCuoiKy_QuanLyHieuThuoc.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Login(string TenDangNhap, string MatKhau)
        {
            string role = "";
            string redirectAction = "";
            string redirectController = "";

            // --- [VỊ TRÍ HARDCODE LOGIC PHÂN QUYỀN] ---
            if (TenDangNhap == "admin" && MatKhau == "admin")
            {
                role = "QuanLy";
                redirectAction = "Index_Admin";
                redirectController = "DashBoard";
            }
            else if (TenDangNhap == "nhanvienkho" && MatKhau == "nhanvienkho")
            {
                role = "NhanVienKho";
                redirectAction = "Index";
                redirectController = "TongQuanKho";
            }
            else if (TenDangNhap == "nhanvienquay" && MatKhau == "nhanvienquay")
            {
                role = "DuocSi";
                redirectAction = "Index";
                redirectController = "TongQuanCaLam";
            }

            /* [NOTE SQL/LINQ THỰC TẾ]:
            var taiKhoan = _context.TaiKhoans.Include(t => t.MaVaiTroNavigation)
                           .FirstOrDefault(u => u.TenDangNhap == TenDangNhap && u.MatKhau == MatKhau);
            if (taiKhoan != null) {
                role = taiKhoan.MaVaiTroNavigation.TenVaiTro; // "QuanLy", "DuocSi"...
                // Chuyển hướng dựa trên MaVaiTro
            }
            */

            if (!string.IsNullOrEmpty(role))
            {
                // Tạo "thẻ bài" định danh người dùng và vai trò
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, TenDangNhap),
                    new Claim(ClaimTypes.Role, role) // Lưu vai trò ở đây
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction(redirectAction, redirectController);
            }

            ViewBag.Error = "Tài khoản hoặc mật khẩu không đúng!";
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
