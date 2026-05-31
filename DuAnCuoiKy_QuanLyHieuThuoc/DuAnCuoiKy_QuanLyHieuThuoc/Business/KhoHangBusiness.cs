using DuAnCuoiKy_QuanLyHieuThuoc.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Business
{
    public class KhoHangBusiness
    {
        private readonly HieuThuocDbContext _context;

        public KhoHangBusiness(HieuThuocDbContext context)
        {
            _context = context;
        }

        public (List<SanPhamKho> Data, int TotalCount, List<string> Categories) GetTonKhoThucTe(string keyword, string loai, int page, int pageSize)
        {
            // 1. Kéo dữ liệu từ Bảng SanPham (Kèm ĐVT, Lô Hàng, và thông tin Thuốc/Vật Tư)
            var query = _context.SanPhams
                .Include(sp => sp.MaDvtNavigation)
                .Include(sp => sp.LoHangs)
                .Include(sp => sp.Thuoc)
                .Include(sp => sp.VatTuYte)
                .AsQueryable();

            // 2. Lọc theo từ khóa (Tìm tên sản phẩm)
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string kw = keyword.Trim().ToLower();
                query = query.Where(sp => sp.TenSp.ToLower().Contains(kw));
            }

            // Kéo toàn bộ bảng danh mục về RAM để xử lý nhanh (Không gây nghẽn SQL)
            var listSanPhams = query.ToList();
            var listLoaiThuoc = _context.LoaiThuocs.ToList();
            var listLoaiVatTu = _context.LoaiVatTus.ToList();

            // 3. Bóc tách dữ liệu và xác định Loại là Thuốc hay Vật Tư
            var allProducts = listSanPhams.Select(sp =>
            {
                string tenLoai = "Khác";

                // Tìm tên loại nếu Sản phẩm này là Thuốc
                if (sp.Thuoc != null)
                {
                    var lt = listLoaiThuoc.FirstOrDefault(l => l.MaLoai == sp.Thuoc.MaLoai);
                    if (lt != null) tenLoai = lt.TenLoai;
                }
                // Tìm tên loại nếu Sản phẩm này là Vật Tư Y Tế
                else if (sp.VatTuYte != null)
                {
                    var lv = listLoaiVatTu.FirstOrDefault(l => l.MaLoaiVt == sp.VatTuYte.MaLoaiVt);
                    if (lv != null) tenLoai = lv.TenLoaiVt;
                }

                // Tính tổng Tồn Kho từ tất cả các Lô Hàng
                int tongTonKho = sp.LoHangs?.Sum(lh => lh.SoLuongConLai) ?? 0;

                return new SanPhamKho
                {
                    TenThuoc = sp.TenSp,
                    Loai = tenLoai,
                    DonViTinh = sp.MaDvtNavigation?.TenDvt ?? "",
                    TonKho = tongTonKho,
                    TrangThai = DetermineStatus(tongTonKho)
                };
            }).ToList();

            // 4. Lọc theo danh mục (Dropdown Lọc)
            if (!string.IsNullOrWhiteSpace(loai))
            {
                allProducts = allProducts.Where(p => p.Loai == loai).ToList();
            }

            // 5. Gộp chung danh sách Categories cho Dropdown (Gồm cả Thuốc + Vật Tư)
            var categories = new List<string>();
            categories.AddRange(listLoaiThuoc.Select(lt => lt.TenLoai));
            categories.AddRange(listLoaiVatTu.Select(lv => lv.TenLoaiVt));
            categories = categories.Distinct().OrderBy(x => x).ToList();

            // 6. Thuật toán Phân trang
            int totalCount = allProducts.Count;
            var pagedData = allProducts
                .OrderBy(x => x.TenThuoc) // Sắp xếp theo tên A-Z
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return (pagedData, totalCount, categories);
        }

        // Logic tự động xếp loại Trạng thái theo số lượng tồn
        private string DetermineStatus(int tonKho)
        {
            if (tonKho <= 0) return "Het hang";
            if (tonKho <= 20) return "Sap het";
            if (tonKho <= 100) return "Binh thuong";
            return "San sang";
        }
    }
}