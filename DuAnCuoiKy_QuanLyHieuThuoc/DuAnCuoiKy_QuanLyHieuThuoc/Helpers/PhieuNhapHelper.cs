using DuAnCuoiKy_QuanLyHieuThuoc.Models;
using DuAnCuoiKy_QuanLyHieuThuoc.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Helpers
{
    public static class PhieuNhapHelper
    {
        public static PhieuNhapCreateViewModel TaoViewModel(HieuThuocDbContext context)
        {
            return new PhieuNhapCreateViewModel
            {
                DsNhaCungCap = context.NhaCungCaps
                    .Select(x => new SelectListItem
                    {
                        Value = x.MaNcc,
                        Text = x.TenNcc
                    })
                    .ToList(),

                DsSanPham = context.SanPhams
                    .Select(x => new SelectListItem
                    {
                        Value = x.MaSp,
                        Text = x.TenSp
                    })
                    .ToList()
            };
        }

        public static PhieuNhapCreateViewModel NapLaiCombobox(
            HieuThuocDbContext context,
            PhieuNhapCreateViewModel vm)
        {
            vm.DsNhaCungCap = context.NhaCungCaps
                .Select(x => new SelectListItem
                {
                    Value = x.MaNcc,
                    Text = x.TenNcc
                })
                .ToList();

            vm.DsSanPham = context.SanPhams
                .Select(x => new SelectListItem
                {
                    Value = x.MaSp,
                    Text = x.TenSp
                })
                .ToList();

            return vm;
        }

        public static string SinhMaPhieu(HieuThuocDbContext context)
        {
            var dsSo = context.PhieuNhaps
                .Select(x => x.MaPhieuNhap)
                .Where(x => x.StartsWith("PN") && x.Length == 6)
                .Select(x => int.Parse(x.Substring(2)))
                .ToList();

            int soMoi = dsSo.Any() ? dsSo.Max() + 1 : 1;
            return $"PN{soMoi:0000}";
        }
    }
}