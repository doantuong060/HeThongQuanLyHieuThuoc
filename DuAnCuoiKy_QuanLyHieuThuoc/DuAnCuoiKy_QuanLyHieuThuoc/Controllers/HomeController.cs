using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using DuAnCuoiKy_QuanLyHieuThuoc.Models.ViewModels;
using DuAnCuoiKy_QuanLyHieuThuoc.Business; // Dùng Business layer

namespace DuAnCuoiKy_QuanLyHieuThuoc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAccountService _accountService; // Tiêm Service vào

        public HomeController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            // [GỌI BUSINESS LAYER]
            var taiKhoan = await _accountService.AuthenticateAsync(model.TenDangNhap, model.MatKhau);

            if (taiKhoan != null)
            {
                if (taiKhoan.MaNvNavigation.TrangThai == false)
                {
                    ModelState.AddModelError("", "Tài khoản bị khóa.");
                    return View(model);
                }

                // Thiết lập Claims
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, taiKhoan.MaNvNavigation.HoTen),
                    new Claim(ClaimTypes.Role, taiKhoan.MaVaiTroNavigation.TenVaiTro),
                    new Claim("MaNV", taiKhoan.MaNv)
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)));

                // Cập nhật login qua Service
                await _accountService.UpdateLastLoginAsync(taiKhoan.MaNv);

                // Điều hướng theo Role
                string role = taiKhoan.MaVaiTroNavigation.TenVaiTro;
                return role switch
                {
                    "QuanLy" => RedirectToAction("Index_Admin", "Dashboard"),
                    "NhanVienKho" => RedirectToAction("Index", "TongQuanKho"),
                    _ => RedirectToAction("Index", "TongQuanCaLam")
                };
            }

            ModelState.AddModelError("", "Sai tài khoản hoặc mật khẩu.");
            return View(model);
        }
    }
}