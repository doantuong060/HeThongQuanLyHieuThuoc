using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Models.ViewModels
{
    public class PhieuNhapCreateViewModel
    {
        [Required]
        public string MaNcc { get; set; } = "";

        public List<SelectListItem> DsNhaCungCap { get; set; }
            = new List<SelectListItem>();

        public List<SelectListItem> DsSanPham { get; set; }
            = new List<SelectListItem>();

        public List<LoHangNhapVM> DanhSachLoHang { get; set; }
            = new List<LoHangNhapVM>();
    }

    public class LoHangNhapVM
    {
        [Required]
        public string MaSp { get; set; } = "";

        [Required]
        public string SoLo { get; set; } = "";

        [Range(1, double.MaxValue,
            ErrorMessage = "Giá nhập phải > 0")]
        public decimal GiaNhap { get; set; }

        [Required]
        public DateOnly HanSuDung { get; set; }

        [Range(1, int.MaxValue,
            ErrorMessage = "Số lượng phải > 0")]
        public int SoLuongNhap { get; set; }
    }
}