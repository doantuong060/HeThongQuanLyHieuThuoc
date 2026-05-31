using DuAnCuoiKy_QuanLyHieuThuoc.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Business
{
    public class LichSuHoaDonBusiness
    {
        private readonly HieuThuocDbContext _context;

        public LichSuHoaDonBusiness(HieuThuocDbContext context)
        {
            _context = context;
        }

        // Thêm tham số page (trang hiện tại) và pageSize (số lượng/trang)
        // Trả về một Tuple gồm: Danh sách dữ liệu (Data) và Tổng số lượng (TotalCount)
        public (List<HoaDon> Data, int TotalCount) GetLichSuHoaDon(string searchKeyword, string timeFilter, string statusFilter, int page, int pageSize)
        {
            var query = _context.HoaDons
                .Include(h => h.ChiTietHoaDons)
                    .ThenInclude(ct => ct.SoLoNavigation)
                        .ThenInclude(sl => sl.MaSpNavigation)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchKeyword))
            {
                string keyword = searchKeyword.Trim().ToLower();
                query = query.Where(h => h.MaHd.ToLower().Contains(keyword)
                                      || h.ChiTietHoaDons.Any(ct => ct.SoLoNavigation.MaSpNavigation.TenSp.ToLower().Contains(keyword)));
            }

            if (!string.IsNullOrWhiteSpace(timeFilter) && timeFilter != "all")
            {
                var today = DateTime.Today;
                if (timeFilter == "today")
                {
                    query = query.Where(h => h.NgayBan >= today && h.NgayBan < today.AddDays(1));
                }
                else if (timeFilter == "yesterday")
                {
                    query = query.Where(h => h.NgayBan >= today.AddDays(-1) && h.NgayBan < today);
                }
                else if (timeFilter == "7days")
                {
                    query = query.Where(h => h.NgayBan >= today.AddDays(-7) && h.NgayBan < today.AddDays(1));
                }
            }

            if (!string.IsNullOrWhiteSpace(statusFilter))
            {
                if (statusFilter == "cancelled")
                {
                    query = query.Where(h => h.GhiChu != null && h.GhiChu.Contains("hủy"));
                }
                else if (statusFilter == "success")
                {
                    query = query.Where(h => h.GhiChu == null || !h.GhiChu.Contains("hủy"));
                }
            }

            // Đếm tổng số bản ghi thỏa mãn điều kiện lọc
            int totalCount = query.Count();

            // Sắp xếp và cắt đúng số lượng của trang hiện tại
            var data = query.OrderByDescending(h => h.NgayBan)
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToList();

            return (data, totalCount);
        }
    }
}