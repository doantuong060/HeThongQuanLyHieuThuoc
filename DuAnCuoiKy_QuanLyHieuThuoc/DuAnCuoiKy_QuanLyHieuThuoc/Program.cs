<<<<<<< HEAD
using DuAnCuoiKy_QuanLyHieuThuoc.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- 1. THÊM DỊCH VỤ SESSION VÀO ĐÂY ---
builder.Services.AddDistributedMemoryCache(); // Cần thiết để lưu session vào bộ nhớ
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddControllersWithViews();
=======
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using DuAnCuoiKy_QuanLyHieuThuoc.Models;
using DuAnCuoiKy_QuanLyHieuThuoc.Business;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

>>>>>>> a1acad9 (Hoan thien giao dien tong quan kho)
builder.Services.AddDbContext<HieuThuocDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
<<<<<<< HEAD
    .AddCookie(options => { options.LoginPath = "/Home/Login"; });

var app = builder.Build();

// Configure the HTTP request pipeline.
=======
    .AddCookie(options =>
    {
        options.LoginPath = "/Home/Login";          // Đường dẫn trang đăng nhập
        options.LogoutPath = "/Home/Logout";        // Đường dẫn trang đăng xuất
        options.AccessDeniedPath = "/Home/Login";   // Trang hiện ra khi vào phần không có quyền
        options.ExpireTimeSpan = TimeSpan.FromHours(8); // Cookie có hiệu lực trong 8 tiếng
        options.Cookie.HttpOnly = true;             // Bảo mật cookie khỏi script lạ
    });

builder.Services.AddScoped<IAccountService, AccountService>();


var app = builder.Build();

>>>>>>> a1acad9 (Hoan thien giao dien tong quan kho)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
<<<<<<< HEAD
app.UseStaticFiles(); // Đảm bảo đã có dòng này thay vì chỉ dùng MapStaticAssets

app.UseRouting();
app.UseSession();        
app.UseAuthentication(); 
app.UseAuthorization();  
=======
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
>>>>>>> a1acad9 (Hoan thien giao dien tong quan kho)

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}/{id?}");

app.Run();