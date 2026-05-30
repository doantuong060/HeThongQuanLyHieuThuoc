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
    public class LoHangsController : Controller
    {
        private readonly HieuThuocDbContext _context;

        public LoHangsController(HieuThuocDbContext context)
        {
            _context = context;
        }

        // GET: LoHangs
        public async Task<IActionResult> Index()
        {
            var hieuThuocDbContext = _context.LoHangs.Include(l => l.MaPhieuNhapNavigation).Include(l => l.MaSpNavigation);
            return View(await hieuThuocDbContext.ToListAsync());
        }

        // GET: LoHangs/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loHang = await _context.LoHangs
                .Include(l => l.MaPhieuNhapNavigation)
                .Include(l => l.MaSpNavigation)
                .FirstOrDefaultAsync(m => m.SoLo == id);
            if (loHang == null)
            {
                return NotFound();
            }

            return View(loHang);
        }

        // GET: LoHangs/Create
        public IActionResult Create()
        {
            ViewData["MaPhieuNhap"] = new SelectList(_context.PhieuNhaps, "MaPhieuNhap", "MaPhieuNhap");
            ViewData["MaSp"] = new SelectList(_context.SanPhams, "MaSp", "MaSp");
            return View();
        }

        // POST: LoHangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SoLo,MaSp,MaPhieuNhap,GiaNhap,HanSuDung,SoLuongNhap,SoLuongConLai")] LoHang loHang)
        {
            if (ModelState.IsValid)
            {
                _context.Add(loHang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaPhieuNhap"] = new SelectList(_context.PhieuNhaps, "MaPhieuNhap", "MaPhieuNhap", loHang.MaPhieuNhap);
            ViewData["MaSp"] = new SelectList(_context.SanPhams, "MaSp", "MaSp", loHang.MaSp);
            return View(loHang);
        }

        // GET: LoHangs/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loHang = await _context.LoHangs.FindAsync(id);
            if (loHang == null)
            {
                return NotFound();
            }
            ViewData["MaPhieuNhap"] = new SelectList(_context.PhieuNhaps, "MaPhieuNhap", "MaPhieuNhap", loHang.MaPhieuNhap);
            ViewData["MaSp"] = new SelectList(_context.SanPhams, "MaSp", "MaSp", loHang.MaSp);
            return View(loHang);
        }

        // POST: LoHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("SoLo,MaSp,MaPhieuNhap,GiaNhap,HanSuDung,SoLuongNhap,SoLuongConLai")] LoHang loHang)
        {
            if (id != loHang.SoLo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loHang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoHangExists(loHang.SoLo))
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
            ViewData["MaPhieuNhap"] = new SelectList(_context.PhieuNhaps, "MaPhieuNhap", "MaPhieuNhap", loHang.MaPhieuNhap);
            ViewData["MaSp"] = new SelectList(_context.SanPhams, "MaSp", "MaSp", loHang.MaSp);
            return View(loHang);
        }

        // GET: LoHangs/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loHang = await _context.LoHangs
                .Include(l => l.MaPhieuNhapNavigation)
                .Include(l => l.MaSpNavigation)
                .FirstOrDefaultAsync(m => m.SoLo == id);
            if (loHang == null)
            {
                return NotFound();
            }

            return View(loHang);
        }

        // POST: LoHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var loHang = await _context.LoHangs.FindAsync(id);
            if (loHang != null)
            {
                _context.LoHangs.Remove(loHang);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoHangExists(string id)
        {
            return _context.LoHangs.Any(e => e.SoLo == id);
        }
    }
}
