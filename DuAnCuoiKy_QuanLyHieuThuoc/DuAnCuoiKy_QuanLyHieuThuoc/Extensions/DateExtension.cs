using System;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Extensions
{
    public static class DateExtension
    {
        // Tính số ngày đến hạn dùng
        public static int TinhSoNgayDenHan(this DateTime hanSuDung)
        {
            TimeSpan diff = hanSuDung.Date - DateTime.Today;
            return diff.Days;
        }
    }
}