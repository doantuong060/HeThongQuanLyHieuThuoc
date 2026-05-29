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

        public List<VwTonKhoSanPham> GetDanhSachSanPham(string keyword, string loai)
        {
            var query = _context.VwTonKhoSanPhams.Where(sp => sp.TrangThai == true).AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(x => x.TenSp.Contains(keyword) || x.MaSp.Contains(keyword));
            }

            if (!string.IsNullOrWhiteSpace(loai))
            {
                var thuocQuery = _context.Thuocs.AsQueryable();
                // FIX CS8602: Đã thêm check MaLoaiNavigation != null
                query = query.Where(sp => thuocQuery.Any(t => t.MaSp == sp.MaSp && t.MaLoaiNavigation != null && t.MaLoaiNavigation.TenLoai.Contains(loai)));
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

        // Đã thêm tham số ghiChuThanhToan để lưu phương thức (Tiền mặt/Chuyển khoản...)
        public string ThanhToanDonHang(List<CartItem> cart, string maNV, string ghiChuThanhToan)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                string maHD = "HD" + DateTime.Now.ToString("yyMMddHHmmss");

                var hoaDon = new HoaDon
                {
                    MaHd = maHD,
                    NgayBan = DateTime.Now,
                    MaNv = maNV,
                    GhiChu = ghiChuThanhToan // Lưu Enum vào đây
                };
                _context.HoaDons.Add(hoaDon);
                _context.SaveChanges();

                int stt = 1;
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

                        var cthd = new ChiTietHoaDon
                        {
                            MaCthd = maHD + "-" + stt.ToString("D2"),
                            MaHd = maHD,
                            SoLo = lo.SoLo,
                            SoLuong = soLuongXuatTuLo,
                            DonGia = item.GiaBan
                        };
                        _context.ChiTietHoaDons.Add(cthd);

                        soLuongThieu -= soLuongXuatTuLo;
                        stt++;
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