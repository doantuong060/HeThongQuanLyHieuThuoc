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
    public class TaiKhoansController : Controller
    {
        private readonly HieuThuocDbContext _context;

        public TaiKhoansController(HieuThuocDbContext context)
        {
            _context = context;
        }

        // GET: TaiKhoans
        public async Task<IActionResult> Index()
        {
            var hieuThuocDbContext = _context.TaiKhoans.Include(t => t.MaNvNavigation).Include(t => t.MaVaiTroNavigation);
            return View(await hieuThuocDbContext.ToListAsync());
        }

        // GET: TaiKhoans/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taiKhoan = await _context.TaiKhoans
                .Include(t => t.MaNvNavigation)
                .Include(t => t.MaVaiTroNavigation)
                .FirstOrDefaultAsync(m => m.MaNv == id);
            if (taiKhoan == null)
            {
                return NotFound();
            }

            return View(taiKhoan);
        }

        // GET: TaiKhoans/Create
        public IActionResult Create()
        {
            ViewData["MaNv"] = new SelectList(_context.NhanViens, "MaNv", "MaNv");
            ViewData["MaVaiTro"] = new SelectList(_context.VaiTros, "MaVaiTro", "MaVaiTro");
            return View();
        }

        // POST: TaiKhoans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaNv,TenDangNhap,MatKhau,NgayTao,LanDangNhapCuoi,MaVaiTro")] TaiKhoan taiKhoan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(taiKhoan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaNv"] = new SelectList(_context.NhanViens, "MaNv", "MaNv", taiKhoan.MaNv);
            ViewData["MaVaiTro"] = new SelectList(_context.VaiTros, "MaVaiTro", "MaVaiTro", taiKhoan.MaVaiTro);
            return View(taiKhoan);
        }

        // GET: TaiKhoans/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taiKhoan = await _context.TaiKhoans.FindAsync(id);
            if (taiKhoan == null)
            {
                return NotFound();
            }
            ViewData["MaNv"] = new SelectList(_context.NhanViens, "MaNv", "MaNv", taiKhoan.MaNv);
            ViewData["MaVaiTro"] = new SelectList(_context.VaiTros, "MaVaiTro", "MaVaiTro", taiKhoan.MaVaiTro);
            return View(taiKhoan);
        }

        // POST: TaiKhoans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaNv,TenDangNhap,MatKhau,NgayTao,LanDangNhapCuoi,MaVaiTro")] TaiKhoan taiKhoan)
        {
            if (id != taiKhoan.MaNv)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taiKhoan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaiKhoanExists(taiKhoan.MaNv))
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
            ViewData["MaNv"] = new SelectList(_context.NhanViens, "MaNv", "MaNv", taiKhoan.MaNv);
            ViewData["MaVaiTro"] = new SelectList(_context.VaiTros, "MaVaiTro", "MaVaiTro", taiKhoan.MaVaiTro);
            return View(taiKhoan);
        }

        // GET: TaiKhoans/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taiKhoan = await _context.TaiKhoans
                .Include(t => t.MaNvNavigation)
                .Include(t => t.MaVaiTroNavigation)
                .FirstOrDefaultAsync(m => m.MaNv == id);
            if (taiKhoan == null)
            {
                return NotFound();
            }

            return View(taiKhoan);
        }

        // POST: TaiKhoans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var taiKhoan = await _context.TaiKhoans.FindAsync(id);
            if (taiKhoan != null)
            {
                _context.TaiKhoans.Remove(taiKhoan);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaiKhoanExists(string id)
        {
            return _context.TaiKhoans.Any(e => e.MaNv == id);
        }
    }
}
