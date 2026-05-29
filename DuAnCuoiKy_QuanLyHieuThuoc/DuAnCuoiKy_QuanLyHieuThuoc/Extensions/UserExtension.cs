using System.Security.Claims;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Extensions
{
    public static class UserExtension
    {
        // Lấy MaNV
        public static string GetMaNV(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
        }

        // Lấy HoTen
        public static string GetHoTen(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.Name) ?? "Chưa rõ";
        }

        // Lấy Role
        public static string GetRole(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.Role) ?? "";
        }
    }
}