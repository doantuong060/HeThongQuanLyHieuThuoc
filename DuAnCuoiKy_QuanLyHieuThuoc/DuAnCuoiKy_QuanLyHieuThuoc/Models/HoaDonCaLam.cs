namespace DuAnCuoiKy_QuanLyHieuThuoc.Models
{
    public class HoaDonCaLam
    {
        public string MaHD { get; set; } = "";

        public string ThoiGian { get; set; } = "";

        public decimal TongTien { get; set; }

        /// <summary>
        /// "Hoàn thành" | "Chờ thanh toán" | "Đã hủy"
        /// </summary>
        public string TrangThai { get; set; } = "";
    }
}