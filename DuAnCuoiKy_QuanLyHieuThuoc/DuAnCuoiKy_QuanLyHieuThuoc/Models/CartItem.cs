namespace DuAnCuoiKy_QuanLyHieuThuoc.Models
{
    public class CartItem
    {
        public string MaSP { get; set; } = "";

        public string TenSP { get; set; } = "";

        public decimal GiaBan { get; set; }

        public int SoLuong { get; set; }

        public int TonKho { get; set; }

        public decimal ThanhTien
        {
            get
            {
                return GiaBan * SoLuong;
            }
        }
    }
}