<<<<<<< HEAD
using DuAnCuoiKy_QuanLyHieuThuoc.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
=======
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using DuAnCuoiKy_QuanLyHieuThuoc.Models.ViewModels;
using DuAnCuoiKy_QuanLyHieuThuoc.Business; // Dùng Business layer
>>>>>>> a1acad9 (Hoan thien giao dien tong quan kho)

namespace DuAnCuoiKy_QuanLyHieuThuoc.Controllers
{
    public class HomeController : Controller
    {
<<<<<<< HEAD
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
=======
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
>>>>>>> a1acad9 (Hoan thien giao dien tong quan kho)
