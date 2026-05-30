using DuAnCuoiKy_QuanLyHieuThuoc.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// --- [VỊ TRÍ SỬA ĐIỀU HƯỚNG MẶC ĐỊNH] ---
// Đổi Home/Index thành TaiKhoans/Login để web mở trang Đăng nhập trước
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=TaiKhoans}/{action=Login}/{id?}");

app.Run();