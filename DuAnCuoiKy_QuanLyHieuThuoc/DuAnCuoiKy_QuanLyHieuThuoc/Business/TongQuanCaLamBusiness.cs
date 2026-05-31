using DuAnCuoiKy_QuanLyHieuThuoc.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Business
{
    public class TongQuanCaLamBusiness
    {
        private readonly HieuThuocDbContext _context;
        public TongQuanCaLamBusiness(HieuThuocDbContext context) => _context = context;

        public (int SoHoaDon, decimal DoanhThu, int SoKhach, int[] ChartData) GetThongKeTrongNgay(string maNV)
        {
            DateTime today = DateTime.Today;
            DateTime tomorrow = today.AddDays(1);
            var hoaDonsToday = _context.HoaDons.Where(hd => hd.MaNv == maNV && hd.NgayBan >= today && hd.NgayBan < tomorrow).ToList();

            int[] data = new int[12];
            foreach (var hd in hoaDonsToday)
            {
                int hour = hd.NgayBan.Hour;
                if (hour >= 7 && hour <= 18) data[hour - 7]++;
            }
            return (hoaDonsToday.Count, hoaDonsToday.Sum(hd => hd.TongTien ?? 0), hoaDonsToday.Count, data);
        }

        public List<HoaDonCaLam> GetHoaDonGanNhat(string maNV, int take = 5)
        {
            DateTime today = DateTime.Today;
            DateTime tomorrow = today.AddDays(1);
            return _context.HoaDons.Where(hd => hd.MaNv == maNV && hd.NgayBan >= today && hd.NgayBan < tomorrow)
                .OrderByDescending(hd => hd.NgayBan).Take(take)
                .Select(hd => new HoaDonCaLam { MaHD = hd.MaHd, ThoiGian = hd.NgayBan.ToString("hh:mm tt"), TongTien = hd.TongTien ?? 0, TrangThai = "Hoàn thành" }).ToList();
        }

        public List<HoaDonCaLam> GetAllHoaDonTrongNgay(string maNV)
        {
            DateTime today = DateTime.Today;
            DateTime tomorrow = today.AddDays(1);
            return _context.HoaDons.Where(hd => hd.MaNv == maNV && hd.NgayBan >= today && hd.NgayBan < tomorrow)
                .OrderByDescending(hd => hd.NgayBan)
                .Select(hd => new HoaDonCaLam { MaHD = hd.MaHd, ThoiGian = hd.NgayBan.ToString("hh:mm tt"), TongTien = hd.TongTien ?? 0, TrangThai = "Hoàn thành" }).ToList();
        }

        public object GetChiTietHoaDon(string maHD)
        {
            var chiTiet = _context.ChiTietHoaDons.Where(ct => ct.MaHd == maHD).Select(ct => new {
                TenSP = ct.SoLoNavigation.MaSpNavigation.TenSp,
                DVT = ct.SoLoNavigation.MaSpNavigation.MaDvtNavigation.TenDvt,
                SoLuong = ct.SoLuong,
                DonGia = ct.DonGia,
                ThanhTien = ct.SoLuong * ct.DonGia
            }).ToList();
            var hoaDon = _context.HoaDons.FirstOrDefault(hd => hd.MaHd == maHD);
            return new { MaHD = maHD, NgayBan = hoaDon?.NgayBan.ToString("dd/MM/yyyy - HH:mm"), TongTien = hoaDon?.TongTien ?? 0, ChiTiet = chiTiet };
        }
    }
}