using System.ComponentModel.DataAnnotations;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Models.ViewModels
{
    public class AddSupplierViewModel
    {
        [Required(ErrorMessage = "Tên nhà cung cấp không được để trống")]
        [StringLength(100, ErrorMessage = "Tên không được quá 100 ký tự")]
        public string TenNcc { get; set; } = null!;

        [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
        [RegularExpression(@"^\d{10,11}$", ErrorMessage = "Số điện thoại không hợp lệ (10-11 số)")]
        public string? SoDienThoai { get; set; }

        [EmailAddress(ErrorMessage = "Địa chỉ Email không hợp lệ")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn Tỉnh/Thành phố")]
        public string MaTinhThanh { get; set; } = null!;

        [Display(Name = "Địa chỉ chi tiết")]
        public string? DiaChiChiTiet { get; set; }
    }
}