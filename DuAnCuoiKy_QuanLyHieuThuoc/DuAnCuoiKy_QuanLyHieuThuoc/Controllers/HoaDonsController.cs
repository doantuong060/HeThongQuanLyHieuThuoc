using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DuAnCuoiKy_QuanLyHieuThuoc.Models;

namespace DuAnCuoiKy_QuanLyHieuThuoc.Controllers
{
    public class HoaDonsController : Controller
    {
        private readonly HieuThuocDbContext _context;

        public HoaDonsController(HieuThuocDbContext context)
        {
            _context = context;
        }

        // GET: HoaDons
        // Tiếp nhận 3 tham số lọc truyền từ Giao diện qua URL query string
        public IActionResult Index(string searchKeyword, string timeFilter, string statusFilter)
        {
            // 1. Khởi tạo danh sách gốc (Mock data)
            var danhSach = new List<HoaDon>
            {
                new HoaDon { MaHd = "HD0001", NgayBan = DateTime.Now.AddMinutes(-15), TongTien = 45000, GhiChu = "Amoxicillin 500mg, Paracetamol 500mg" },
                new HoaDon { MaHd = "HD0002", NgayBan = DateTime.Now.AddHours(-2), TongTien = 120000, GhiChu = "Augmentin 625mg, Vitamin C 1000mg, Panadol Extra" },
                new HoaDon { MaHd = "HD0003", NgayBan = DateTime.Now.AddDays(-1).AddHours(3), TongTien = 85000, GhiChu = "Cefuroxim 500mg, Berberin 100mg" },
                new HoaDon { MaHd = "HD0004", NgayBan = DateTime.Now.AddDays(-1).AddHours(-2), TongTien = 210000, GhiChu = "Telfast 180mg, Hỗn dịch dạ dày Gaviscon" },
                new HoaDon { MaHd = "HD0005", NgayBan = DateTime.Now.AddDays(-2), TongTien = 35000, GhiChu = "Efferalgan Codeine 500mg" }
            };

            // 2. LỌC THEO TỪ KHÓA (Tìm theo Mã HĐ hoặc Tên thuốc có trong Ghi chú)
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                danhSach = danhSach.Where(h => h.MaHd.Contains(searchKeyword, StringComparison.OrdinalIgnoreCase)
                                            || (h.GhiChu != null && h.GhiChu.Contains(searchKeyword, StringComparison.OrdinalIgnoreCase))).ToList();
            }

            // 3. LỌC THEO THỜI GIAN
            if (!string.IsNullOrEmpty(timeFilter) && timeFilter != "all")
            {
                var today = DateTime.Today;
                if (timeFilter == "today")
                {
                    danhSach = danhSach.Where(h => h.NgayBan.Date == today).ToList();
                }
                else if (timeFilter == "yesterday")
                {
                    danhSach = danhSach.Where(h => h.NgayBan.Date == today.AddDays(-1)).ToList();
                }
                else if (timeFilter == "7days")
                {
                    danhSach = danhSach.Where(h => h.NgayBan.Date >= today.AddDays(-7)).ToList();
                }
            }

            // 4. LỌC THEO TRẠNG THÁI
            if (!string.IsNullOrEmpty(statusFilter))
            {
                if (statusFilter == "cancelled")
                {
                    // Lọc các hóa đơn có chữ "hủy" trong ghi chú
                    danhSach = danhSach.Where(h => h.GhiChu != null && h.GhiChu.ToLower().Contains("hủy")).ToList();
                }
                else if (statusFilter == "success")
                {
                    // Lọc các hóa đơn thông thường (hoàn thành)
                    danhSach = danhSach.Where(h => h.GhiChu == null || !h.GhiChu.ToLower().Contains("hủy")).ToList();
                }
            }

            // 5. Gửi ngược các giá trị đang lọc về View để giữ trạng thái hiển thị trên các ô Nhập/Chọn
            ViewBag.SearchKeyword = searchKeyword;
            ViewBag.TimeFilter = timeFilter;
            ViewBag.StatusFilter = statusFilter;

            // Trả về danh sách sau khi đã lọc dữ liệu
            return View(danhSach);
        }

        // GET: HoaDons/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hoaDon = await _context.HoaDons
                .Include(h => h.MaKhNavigation)
                .Include(h => h.MaNvNavigation)
                .FirstOrDefaultAsync(m => m.MaHd == id);
            if (hoaDon == null)
            {
                return NotFound();
            }

            return View(hoaDon);
        }

        // GET: HoaDons/Create
        public IActionResult Create()
        {
            ViewData["MaKh"] = new SelectList(_context.KhachHangs, "MaKh", "MaKh");
            ViewData["MaNv"] = new SelectList(_context.NhanViens, "MaNv", "MaNv");
            return View();
        }

        // POST: HoaDons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaHd,NgayBan,MaNv,MaKh,GhiChu,TongTien")] HoaDon hoaDon)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hoaDon);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaKh"] = new SelectList(_context.KhachHangs, "MaKh", "MaKh", hoaDon.MaKh);
            ViewData["MaNv"] = new SelectList(_context.NhanViens, "MaNv", "MaNv", hoaDon.MaNv);
            return View(hoaDon);
        }

        // GET: HoaDons/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hoaDon = await _context.HoaDons.FindAsync(id);
            if (hoaDon == null)
            {
                return NotFound();
            }
            ViewData["MaKh"] = new SelectList(_context.KhachHangs, "MaKh", "MaKh", hoaDon.MaKh);
            ViewData["MaNv"] = new SelectList(_context.NhanViens, "MaNv", "MaNv", hoaDon.MaNv);
            return View(hoaDon);
        }

        // POST: HoaDons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaHd,NgayBan,MaNv,MaKh,GhiChu,TongTien")] HoaDon hoaDon)
        {
            if (id != hoaDon.MaHd)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hoaDon);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HoaDonExists(hoaDon.MaHd))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaKh"] = new SelectList(_context.KhachHangs, "MaKh", "MaKh", hoaDon.MaKh);
            ViewData["MaNv"] = new SelectList(_context.NhanViens, "MaNv", "MaNv", hoaDon.MaNv);
            return View(hoaDon);
        }

        // GET: HoaDons/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hoaDon = await _context.HoaDons
                .Include(h => h.MaKhNavigation)
                .Include(h => h.MaNvNavigation)
                .FirstOrDefaultAsync(m => m.MaHd == id);
            if (hoaDon == null)
            {
                return NotFound();
            }

            return View(hoaDon);
        }

        // POST: HoaDons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var hoaDon = await _context.HoaDons.FindAsync(id);
            if (hoaDon != null)
            {
                _context.HoaDons.Remove(hoaDon);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HoaDonExists(string id)
        {
            return _context.HoaDons.Any(e => e.MaHd == id);
        }
    }
}
