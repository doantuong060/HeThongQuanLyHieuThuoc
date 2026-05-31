using System.ComponentModel.DataAnnotations;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Models.ViewModels
{
    public class EditProductViewModel
    {
        [Required]
        public string MaSp { get; set; } = null!;

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        public string TenSp { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng chọn đơn vị tính")]
        public string MaDvt { get; set; } = null!;

        [Required(ErrorMessage = "Giá bán là bắt buộc")]
        [Range(100, double.MaxValue, ErrorMessage = "Giá bán phải lớn hơn 100đ")]
        public decimal GiaBan { get; set; }

        public int MucCanhBao { get; set; } = 20;

        // Thông tin THUỐC
        public string? MaLoai { get; set; }
        public bool CanToa { get; set; }
        public string? GhiChu { get; set; }

        // Thông tin VẬT TƯ
        public string? MaLoaiVt { get; set; }
        public string? NhaSanXuat { get; set; }
    }
}
