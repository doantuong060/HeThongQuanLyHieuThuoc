using DuAnCuoiKy_QuanLyHieuThuoc.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Business
{
    public class TongQuanCaLamBusiness
    {
        private readonly HieuThuocDbContext _context;

        public TongQuanCaLamBusiness(HieuThuocDbContext context)
        {
            _context = context;
        }

        public (int SoHoaDon, decimal DoanhThu, int SoKhach, int[] ChartData) GetThongKeTrongNgay(string maNV)
        {
            DateTime today = DateTime.Today;
            var hoaDonsToday = _context.HoaDons
                .Where(hd => hd.MaNv == maNV && hd.NgayBan.Date == today)
                .ToList();

            int[] data = new int[12];
            foreach (var hd in hoaDonsToday)
            {
                int hour = hd.NgayBan.Hour;
                if (hour >= 7 && hour <= 18)
                {
                    data[hour - 7]++;
                }
            }

            return (
                SoHoaDon: hoaDonsToday.Count,
                DoanhThu: hoaDonsToday.Sum(hd => hd.TongTien ?? 0),
                SoKhach: hoaDonsToday.Count,
                ChartData: data
            );
        }

        public List<HoaDonCaLam> GetHoaDonGanNhat(string maNV, int take = 5)
        {
            DateTime today = DateTime.Today;
            return _context.HoaDons
                .Where(hd => hd.MaNv == maNV && hd.NgayBan.Date == today)
                .OrderByDescending(hd => hd.NgayBan)
                .Take(take)
                .Select(hd => new HoaDonCaLam
                {
                    MaHD = hd.MaHd,
                    ThoiGian = hd.NgayBan.ToString("hh:mm tt"),
                    TongTien = hd.TongTien ?? 0,
                    TrangThai = "Hoàn thành"
                })
                .ToList();
        }
    }
}