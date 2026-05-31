using DuAnCuoiKy_QuanLyHieuThuoc.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Business
{
    public class BanHangsBusiness
    {
        private readonly HieuThuocDbContext _context;

        public BanHangsBusiness(HieuThuocDbContext context)
        {
            _context = context;
        }

        // Lấy tất cả danh mục Thuốc và Vật tư từ Database
        public List<string> GetAllDanhMucs()
        {
            var dsThuoc = _context.LoaiThuocs.Select(x => x.TenLoai).ToList();
            var dsVatTu = _context.LoaiVatTus.Select(x => x.TenLoaiVt).ToList();
            return dsThuoc.Concat(dsVatTu).ToList();
        }

        public List<VwTonKhoSanPham> GetDanhSachSanPham(string keyword, string loai)
        {
            var query = _context.VwTonKhoSanPhams.Where(sp => sp.TrangThai == true).AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(x => x.TenSp.Contains(keyword) || x.MaSp.Contains(keyword));
            }

            if (!string.IsNullOrWhiteSpace(loai))
            {
                var thuocQ = _context.Thuocs.AsQueryable();
                var vatTuQ = _context.VatTuYtes.AsQueryable();

                // FIX LOGIC: Tìm trong cả bảng Thuốc VÀ bảng Vật Tư Y Tế
                query = query.Where(sp =>
                    (sp.LoaiSp == "THUOC" && thuocQ.Any(t => t.MaSp == sp.MaSp && t.MaLoaiNavigation != null && t.MaLoaiNavigation.TenLoai.Contains(loai))) ||
                    (sp.LoaiSp == "VATTU" && vatTuQ.Any(v => v.MaSp == sp.MaSp && v.MaLoaiVtNavigation != null && v.MaLoaiVtNavigation.TenLoaiVt.Contains(loai)))
                );
            }

            return query.OrderBy(sp => sp.TenSp).ToList();
        }

        public int GetTonKhoThucTe(string maSP)
        {
            return _context.VwTonKhoSanPhams
                .Where(x => x.MaSp == maSP)
                .Select(x => x.SoLuongTon)
                .FirstOrDefault();
        }

        public string ThanhToanDonHang(List<CartItem> cart, string maNV, string ghiChuThanhToan)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                // ========================================================
                // 1. TẠO MÃ HÓA ĐƠN TỰ ĐỘNG BỌC THÉP
                // ========================================================
                string maHD = "HD0001";
                var maxHD = _context.HoaDons.OrderByDescending(x => x.MaHd).FirstOrDefault();

                if (maxHD != null)
                {
                    // Tự động nhặt tất cả các số có trong chuỗi (Chống lỗi Substring)
                    string numbers = new string(maxHD.MaHd.Where(char.IsDigit).ToArray());
                    if (int.TryParse(numbers, out int currentMaxHD))
                    {
                        maHD = "HD" + (currentMaxHD + 1).ToString("D4");
                        // Nếu lỡ số quá to thì ép cứng về 10 ký tự cho khỏi lỗi SQL
                        if (maHD.Length > 10) maHD = maHD.Substring(0, 10);
                    }
                }

                var hoaDon = new HoaDon
                {
                    MaHd = maHD,
                    NgayBan = DateTime.Now,
                    MaNv = maNV,
                    GhiChu = ghiChuThanhToan
                };
                _context.HoaDons.Add(hoaDon);
                _context.SaveChanges();

                // ========================================================
                // 2. TẠO MÃ CHI TIẾT HÓA ĐƠN BỌC THÉP
                // ========================================================
                int currentMaxCthd = 0;
                var maxCthd = _context.ChiTietHoaDons.OrderByDescending(x => x.MaCthd).FirstOrDefault();

                if (maxCthd != null)
                {
                    string numbers = new string(maxCthd.MaCthd.Where(char.IsDigit).ToArray());
                    int.TryParse(numbers, out currentMaxCthd);
                }

                foreach (var item in cart)
                {
                    var loHangs = _context.LoHangs
                        .Where(l => l.MaSp == item.MaSP && l.SoLuongConLai > 0)
                        .OrderBy(l => l.HanSuDung)
                        .ToList();

                    int soLuongThieu = item.SoLuong;

                    foreach (var lo in loHangs)
                    {
                        if (soLuongThieu <= 0) break;

                        int soLuongXuatTuLo = Math.Min(lo.SoLuongConLai, soLuongThieu);

                        currentMaxCthd++;
                        string maCthd = "CTHD" + currentMaxCthd.ToString("D4");
                        if (maCthd.Length > 10) maCthd = maCthd.Substring(0, 10);

                        var cthd = new ChiTietHoaDon
                        {
                            MaCthd = maCthd,
                            MaHd = maHD,
                            SoLo = lo.SoLo,
                            SoLuong = soLuongXuatTuLo,
                            DonGia = item.GiaBan
                        };
                        _context.ChiTietHoaDons.Add(cthd);

                        soLuongThieu -= soLuongXuatTuLo;
                    }

                    if (soLuongThieu > 0)
                    {
                        throw new Exception($"Sản phẩm '{item.TenSP}' bị thiếu hụt {soLuongThieu} đơn vị so với tồn kho!");
                    }
                }

                _context.SaveChanges();
                transaction.Commit();

                return maHD;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}