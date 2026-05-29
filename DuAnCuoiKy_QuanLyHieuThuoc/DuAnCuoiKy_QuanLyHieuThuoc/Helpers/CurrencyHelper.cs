namespace DuAnCuoiKy_QuanLyHieuThuoc.Helpers
{
    public static class CurrencyHelper
    {
        // Định dạng 1.000.000đ
        public static string FormatVND(decimal amount)
        {
            return amount.ToString("N0") + "đ";
        }

        public static string ToVnd(decimal amount)
        {
            return amount.ToString("N0") + "đ";
        }
    }
}