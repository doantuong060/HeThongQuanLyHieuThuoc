using DuAnCuoiKy_QuanLyHieuThuoc.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Business
{
    public class TongQuanKhoBusiness
    {
        private readonly HieuThuocDbContext _context;

        public TongQuanKhoBusiness(HieuThuocDbContext context)
        {
            _context = context;
        }

        // 1. Lấy tổng số lượng mặt hàng đang kinh doanh
        public int GetTongMatHang()
        {
            return _context.SanPhams.Count(sp => sp.TrangThai == true);
        }

        // 2. Đếm số mặt hàng có tồn kho <= mức cảnh báo
        public int GetSoLuongSapHetHang()
        {
            return _context.VwTonKhoSanPhams.Count(sp => sp.SoLuongTon <= sp.MucCanhBao);
        }

        // 3. Đếm số lô hàng sắp hết hạn (<= 30 ngày) hoặc đã quá hạn
        public int GetSoLuongSapHetHan()
        {
            return _context.VwKiemTraHanDungs.Count(sp => sp.SoNgayConLai <= 30);
        }

        // 4. Lấy danh sách thuốc cần nhập ngay
        public List<VwTonKhoSanPham> GetDsCanNhap(int take = 5)
        {
            return _context.VwTonKhoSanPhams
                .Where(sp => sp.SoLuongTon <= sp.MucCanhBao)
                .OrderBy(sp => sp.SoLuongTon)
                .Take(take)
                .ToList();
        }

        // 5. Lấy danh sách lô hàng cận date
        public List<VwKiemTraHanDung> GetDsHetHan(int take = 5)
        {
            return _context.VwKiemTraHanDungs
                .Where(sp => sp.SoNgayConLai <= 60)
                .OrderBy(sp => sp.SoNgayConLai)
                .Take(take)
                .ToList();
        }

        // 6. Lấy danh sách phiếu nhập kho gần đây
        public List<VwTongTienPhieuNhap> GetDsPhieuNhap(int take = 5)
        {
            return _context.VwTongTienPhieuNhaps
                .OrderByDescending(p => p.NgayNhap)
                .Take(take)
                .ToList();
        }
    }
}