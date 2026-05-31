namespace DuAnCuoiKy_QuanLyHieuThuoc.Helpers
{
    public static class ReportHelper
    {
        public static string FormatShort(decimal value)
        {
            if (value >= 1000000) return (value / 1000000m).ToString("N1") + "M";
            return value.ToString("N0");
        }
    }
}