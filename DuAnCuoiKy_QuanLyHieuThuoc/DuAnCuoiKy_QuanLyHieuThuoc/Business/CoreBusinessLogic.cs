using DuAnCuoiKy_QuanLyHieuThuoc.Models;
using System.Collections.Generic;
using System.Linq;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Business
{
    public class CoreBusinessLogic
    {
        private readonly HieuThuocDbContext _context;

        public CoreBusinessLogic(HieuThuocDbContext context)
        {
            _context = context;
        }

        // 1. Logic trừ kho theo lô (FEFO - First Expire First Out)
        // Thuốc nào hết hạn trước sẽ bị xuất kho trước
        public void TruKhoFEFO(string maSP, int soLuongCanTru)
        {
            // Lấy danh sách lô hàng của sản phẩm, ưu tiên HSD gần nhất (FEFO) và còn số lượng
            var danhSachLo = _context.LoHangs
                .Where(l => l.MaSp == maSP && l.SoLuongConLai > 0)
                .OrderBy(l => l.HanSuDung)
                .ToList();

            int soLuongConThieu = soLuongCanTru;

            foreach (var lo in danhSachLo)
            {
                if (soLuongConThieu <= 0) break;

                if (lo.SoLuongConLai >= soLuongConThieu)
                {
                    // Lô này đủ hàng để trừ
                    lo.SoLuongConLai -= soLuongConThieu;
                    soLuongConThieu = 0;
                }
                else
                {
                    // Lô này không đủ, trừ sạch lô này rồi qua lô tiếp theo
                    soLuongConThieu -= lo.SoLuongConLai;
                    lo.SoLuongConLai = 0;
                }
            }

            _context.SaveChanges();
        }

        // 2. Logic tính điểm tích lũy khách hàng
        // Quy đổi: Cứ 100.000đ = 1 điểm tích lũy
        public decimal TinhDiemTichLuy(string maKH)
        {
            // Lấy tổng tiền các hóa đơn của khách hàng này
            var tongChiTieu = _context.HoaDons
                .Where(hd => hd.MaKh == maKH)
                .Sum(hd => hd.TongTien ?? 0);

            // Công thức tính điểm
            decimal diemTichLuy = Math.Floor(tongChiTieu / 100000);

            return diemTichLuy;
        }
    }
}