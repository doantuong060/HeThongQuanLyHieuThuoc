using System.ComponentModel.DataAnnotations;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Models.ViewModels
{
    public class AddStaffViewModel
    {
        [Required(ErrorMessage = "Họ tên không được để trống")]
        public string HoTen { get; set; } = null!;

        // Nullable để tránh DateTime.MinValue gây lỗi SQL Server
        public DateTime? NgaySinh { get; set; }

        [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
        [RegularExpression(@"^\d{10,11}$", ErrorMessage = "SĐT không hợp lệ (10-11 chữ số)")]
        public string SoDienThoai { get; set; } = null!;

        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string? Email { get; set; }

        public string GioiTinh { get; set; } = "Nam";

        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        // SQL constraint: CK_TK_TenDangNhap - chỉ cho phép a-z, A-Z, 0-9
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Tên đăng nhập chỉ được dùng chữ cái và số, không dấu cách hay ký tự đặc biệt")]
        public string TenDangNhap { get; set; } = null!;

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        // SQL constraint: CK_TK_MatKhau - phải có CHỮ HOA + chữ thường + ký tự @
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*@).{6,}$",
            ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự, gồm chữ HOA, chữ thường và ký tự @. VD: Abc@123")]
        public string MatKhau { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng chọn vai trò")]
        public string MaVaiTro { get; set; } = null!;
    }

    // ViewModel dùng cho form chỉnh sửa nhân viên
    public class EditStaffViewModel
    {
        [Required]
        public string MaNv { get; set; } = null!;

        [Required(ErrorMessage = "Họ tên không được để trống")]
        public string HoTen { get; set; } = null!;

        public DateTime? NgaySinh { get; set; }

        [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
        [RegularExpression(@"^\d{10,11}$", ErrorMessage = "SĐT không hợp lệ (10-11 chữ số)")]
        public string SoDienThoai { get; set; } = null!;

        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string? Email { get; set; }

        public string GioiTinh { get; set; } = "Nam";

        // Vai trò (cập nhật bên bảng TaiKhoan)
        [Required(ErrorMessage = "Vui lòng chọn vai trò")]
        public string MaVaiTro { get; set; } = null!;

        // Mật khẩu mới - không bắt buộc khi sửa, chỉ cập nhật nếu nhập
        // Nếu nhập thì phải đúng rule SQL: CHỮ HOA + chữ thường + @
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*@).{6,}$",
            ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự, gồm chữ HOA, chữ thường và ký tự @. VD: Abc@123")]
        public string? MatKhauMoi { get; set; }
    }
}