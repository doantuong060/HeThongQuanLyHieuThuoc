namespace DuAnCuoiKy_QuanLyHieuThuoc.Models
{
    public class SanPhamKho
    {
        public string TenThuoc { get; set; } = "";
        public string Loai { get; set; } = "";
        public string DonViTinh { get; set; } = "";
        public int TonKho { get; set; }

        /// <summary>
        /// "San sang" | "Binh thuong" | "Sap het" | "Het hang"
        /// </summary>
        public string TrangThai { get; set; } = "";
    }
}
