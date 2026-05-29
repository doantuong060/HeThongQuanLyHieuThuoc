using System;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Helpers
{
    public static class AutoIDHelper
    {
        // Sinh mã theo định dạng: Tiền tố - Năm - Số thứ tự (VD: PN-2024-001)
        public static string GenerateID(string prefix, int currentCount)
        {
            string year = DateTime.Now.Year.ToString();
            // Format số thứ tự thành 3 chữ số (001, 002...)
            string sequence = (currentCount + 1).ToString("D3");

            return $"{prefix}-{year}-{sequence}";
        }
    }
}