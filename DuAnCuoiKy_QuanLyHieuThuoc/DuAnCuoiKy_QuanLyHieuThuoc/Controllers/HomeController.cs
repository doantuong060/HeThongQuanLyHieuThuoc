using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using DuAnCuoiKy_QuanLyHieuThuoc.Models.ViewModels;
using DuAnCuoiKy_QuanLyHieuThuoc.Business;
using DuAnCuoiKy_QuanLyHieuThuoc.Enums;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAccountService _accountService;
        public HomeController(IAccountService accountService) => _accountService = accountService;

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            // GỌI SQL QUA SERVICE
            var taiKhoan = await _accountService.AuthenticateAsync(model.TenDangNhap, model.MatKhau);

            if (taiKhoan == null)
            {
                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không chính xác.");
                return View(model);
            }

            // Kiểm tra trạng thái làm việc (Cột TrangThai trong bảng NhanVien)
            if (taiKhoan.MaNvNavigation.TrangThai == false)
            {
                ModelState.AddModelError("", "Tài khoản này hiện đang bị khóa.");
                return View(model);
            }

            // THIẾT LẬP COOKIE PHIÊN LÀM VIỆC
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, taiKhoan.MaNvNavigation.HoTen),
                new Claim(ClaimTypes.Role, taiKhoan.MaVaiTroNavigation.TenVaiTro),
                new Claim("MaNV", taiKhoan.MaNv),
                new Claim("MaVaiTro", taiKhoan.MaVaiTro) // VT01, VT02...
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), new AuthenticationProperties { IsPersistent = model.GhiNho });

            await _accountService.UpdateLastLoginAsync(taiKhoan.MaNv);

            // ĐIỀU HƯỚNG DỰA TRÊN MÃ VAI TRÒ SQL
            return taiKhoan.MaVaiTro switch
            {
                RoleId.QuanLy => RedirectToAction("Index_Admin", "Dashboard"),
                RoleId.NhanVienKho => RedirectToAction("Index", "TongQuanKho"),
                RoleId.DuocSi => RedirectToAction("Index", "TongQuanCaLam"),
                _ => RedirectToAction("Login")
            };
        }

        // TÍNH NĂNG ĐĂNG XUẤT
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}