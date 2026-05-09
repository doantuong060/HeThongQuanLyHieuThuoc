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
    public class ChiTietNhapsController : Controller
    {
        private readonly HieuThuocDbContext _context;

        public ChiTietNhapsController(HieuThuocDbContext context)
        {
            _context = context;
        }

        // GET: ChiTietNhaps
        public async Task<IActionResult> Index()
        {
            var hieuThuocDbContext = _context.ChiTietNhaps.Include(c => c.MaPhieuNhapNavigation).Include(c => c.MaThuocNavigation);
            return View(await hieuThuocDbContext.ToListAsync());
        }

        // GET: ChiTietNhaps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chiTietNhap = await _context.ChiTietNhaps
                .Include(c => c.MaPhieuNhapNavigation)
                .Include(c => c.MaThuocNavigation)
                .FirstOrDefaultAsync(m => m.MaCtn == id);
            if (chiTietNhap == null)
            {
                return NotFound();
            }

            return View(chiTietNhap);
        }

        // GET: ChiTietNhaps/Create
        public IActionResult Create()
        {
            ViewData["MaPhieuNhap"] = new SelectList(_context.PhieuNhaps, "MaPhieuNhap", "MaPhieuNhap");
            ViewData["MaThuoc"] = new SelectList(_context.Thuocs, "MaThuoc", "MaThuoc");
            return View();
        }

        // POST: ChiTietNhaps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaCtn,MaPhieuNhap,MaThuoc,SoLuong,GiaNhap,SoLo,HanSuDung,ThanhTien")] ChiTietNhap chiTietNhap)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chiTietNhap);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaPhieuNhap"] = new SelectList(_context.PhieuNhaps, "MaPhieuNhap", "MaPhieuNhap", chiTietNhap.MaPhieuNhap);
            ViewData["MaThuoc"] = new SelectList(_context.Thuocs, "MaThuoc", "MaThuoc", chiTietNhap.MaThuoc);
            return View(chiTietNhap);
        }

        // GET: ChiTietNhaps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chiTietNhap = await _context.ChiTietNhaps.FindAsync(id);
            if (chiTietNhap == null)
            {
                return NotFound();
            }
            ViewData["MaPhieuNhap"] = new SelectList(_context.PhieuNhaps, "MaPhieuNhap", "MaPhieuNhap", chiTietNhap.MaPhieuNhap);
            ViewData["MaThuoc"] = new SelectList(_context.Thuocs, "MaThuoc", "MaThuoc", chiTietNhap.MaThuoc);
            return View(chiTietNhap);
        }

        // POST: ChiTietNhaps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaCtn,MaPhieuNhap,MaThuoc,SoLuong,GiaNhap,SoLo,HanSuDung,ThanhTien")] ChiTietNhap chiTietNhap)
        {
            if (id != chiTietNhap.MaCtn)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chiTietNhap);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChiTietNhapExists(chiTietNhap.MaCtn))
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
            ViewData["MaPhieuNhap"] = new SelectList(_context.PhieuNhaps, "MaPhieuNhap", "MaPhieuNhap", chiTietNhap.MaPhieuNhap);
            ViewData["MaThuoc"] = new SelectList(_context.Thuocs, "MaThuoc", "MaThuoc", chiTietNhap.MaThuoc);
            return View(chiTietNhap);
        }

        // GET: ChiTietNhaps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chiTietNhap = await _context.ChiTietNhaps
                .Include(c => c.MaPhieuNhapNavigation)
                .Include(c => c.MaThuocNavigation)
                .FirstOrDefaultAsync(m => m.MaCtn == id);
            if (chiTietNhap == null)
            {
                return NotFound();
            }

            return View(chiTietNhap);
        }

        // POST: ChiTietNhaps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chiTietNhap = await _context.ChiTietNhaps.FindAsync(id);
            if (chiTietNhap != null)
            {
                _context.ChiTietNhaps.Remove(chiTietNhap);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChiTietNhapExists(int id)
        {
            return _context.ChiTietNhaps.Any(e => e.MaCtn == id);
        }
    }
}
