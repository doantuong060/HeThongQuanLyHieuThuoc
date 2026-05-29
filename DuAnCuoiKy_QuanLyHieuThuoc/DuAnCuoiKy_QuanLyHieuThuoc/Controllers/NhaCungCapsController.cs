using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using DuAnCuoiKy_QuanLyHieuThuoc.Models;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Controllers
{
    public class NhaCungCapsController : Controller
    {
        public IActionResult Index()
        {
            // --- [VỊ TRÍ HARDCODE HỆ THỐNG] ---

            // 1. Dữ liệu 3 thẻ đối tác nổi bật (Ảnh 6)
            ViewBag.FeaturedNCC = new List<dynamic>
            {
                new { Ma = "NCC-TRP-01", Ten = "Dược phẩm Traphaco", Loai = "Chiến lược", Status = "Hoạt động", Icon = "building", ColorClass = "primary" },
                new { Ma = "NCC-DHG-02", Ten = "Dược Hậu Giang (DHG)", Loai = "Nội địa", Status = "Hoạt động", Icon = "hospital", ColorClass = "primary" },
                new { Ma = "NCC-VTYT-08", Ten = "Vật tư Y tế Bình Minh", Loai = "Cần rà soát", Status = "Tạm ngưng", Icon = "box-seam", ColorClass = "danger" }
            };

            // 2. Danh sách chi tiết nhà cung cấp
            ViewBag.DanhSachNCC = new List<dynamic>
            {
                new { Ma = "NCC-TRP-01", Ten = "Công ty CP Traphaco", TinhThanh = "Hà Nội", SDT = "024.3681.1111", Status = "Hoạt động", Class = "success" },
                new { Ma = "NCC-DHG-02", Ten = "Dược Hậu Giang", TinhThanh = "Cần Thơ", SDT = "0292.3891.433", Status = "Hoạt động", Class = "success" },
                new { Ma = "NCC-VTYT-08", Ten = "Vật tư Y tế Bình Minh", TinhThanh = "Đà Nẵng", SDT = "0236.3821.555", Status = "Tạm ngưng", Class = "danger" },
                new { Ma = "NCC-OPC-04", Ten = "Công ty CP Dược phẩm OPC", TinhThanh = "TP. HCM", SDT = "028.3875.2048", Status = "Hoạt động", Class = "success" }
            };

            ViewBag.TinhThanh = new SelectList(new[] { "Hà Nội", "TP. Hồ Chí Minh", "Đà Nẵng", "Cần Thơ", "Hải Phòng" });

            return View();
        }
    }
}