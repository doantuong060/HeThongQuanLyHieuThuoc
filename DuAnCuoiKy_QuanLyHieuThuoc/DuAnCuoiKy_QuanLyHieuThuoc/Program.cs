using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using DuAnCuoiKy_QuanLyHieuThuoc.Models;
using DuAnCuoiKy_QuanLyHieuThuoc.Business;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<HieuThuocDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Home/Login";          // Đường dẫn trang đăng nhập
        options.LogoutPath = "/Home/Logout";        // Đường dẫn trang đăng xuất
        options.AccessDeniedPath = "/Home/Login";   // Trang hiện ra khi vào phần không có quyền
        options.ExpireTimeSpan = TimeSpan.FromHours(8); // Cookie có hiệu lực trong 8 tiếng
        options.Cookie.HttpOnly = true;             // Bảo mật cookie khỏi script lạ
    });

builder.Services.AddScoped<IAccountService, AccountService>();

// ========================================================
// BƯỚC 1: ĐĂNG KÝ DỊCH VỤ SESSION (Thêm mới đoạn này)
// ========================================================
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(8); // Giữ giỏ hàng trong 8 tiếng 
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ========================================================
// BƯỚC 2: BẬT MIDDLEWARE SESSION (Phải nằm TRƯỚC UseAuthentication)
// ========================================================
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}/{id?}");

app.Run();