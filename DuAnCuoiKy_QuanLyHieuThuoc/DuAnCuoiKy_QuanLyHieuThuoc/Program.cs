using DuAnCuoiKy_QuanLyHieuThuoc.Models;
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
builder.Services.AddDbContext<HieuThuocDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Đảm bảo đã có dòng này thay vì chỉ dùng MapStaticAssets

app.UseRouting();

// --- 2. KÍCH HOẠT MIDDLEWARE SESSION ---
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=TongQuanCaLam}/{action=Index}/{id?}");

app.Run();