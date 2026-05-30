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
    public class PhuongXasController : Controller
    {
        private readonly HieuThuocDbContext _context;

        public PhuongXasController(HieuThuocDbContext context)
        {
            _context = context;
        }

        // GET: PhuongXas
        public async Task<IActionResult> Index()
        {
            var hieuThuocDbContext = _context.PhuongXas.Include(p => p.MaTinhThanhNavigation);
            return View(await hieuThuocDbContext.ToListAsync());
        }

        // GET: PhuongXas/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phuongXa = await _context.PhuongXas
                .Include(p => p.MaTinhThanhNavigation)
                .FirstOrDefaultAsync(m => m.MaPhuongXa == id);
            if (phuongXa == null)
            {
                return NotFound();
            }

            return View(phuongXa);
        }

        // GET: PhuongXas/Create
        public IActionResult Create()
        {
            ViewData["MaTinhThanh"] = new SelectList(_context.TinhThanhs, "MaTinhThanh", "MaTinhThanh");
            return View();
        }

        // POST: PhuongXas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaPhuongXa,TenPhuongXa,MaTinhThanh")] PhuongXa phuongXa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(phuongXa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaTinhThanh"] = new SelectList(_context.TinhThanhs, "MaTinhThanh", "MaTinhThanh", phuongXa.MaTinhThanh);
            return View(phuongXa);
        }

        // GET: PhuongXas/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phuongXa = await _context.PhuongXas.FindAsync(id);
            if (phuongXa == null)
            {
                return NotFound();
            }
            ViewData["MaTinhThanh"] = new SelectList(_context.TinhThanhs, "MaTinhThanh", "MaTinhThanh", phuongXa.MaTinhThanh);
            return View(phuongXa);
        }

        // POST: PhuongXas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaPhuongXa,TenPhuongXa,MaTinhThanh")] PhuongXa phuongXa)
        {
            if (id != phuongXa.MaPhuongXa)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(phuongXa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhuongXaExists(phuongXa.MaPhuongXa))
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
            ViewData["MaTinhThanh"] = new SelectList(_context.TinhThanhs, "MaTinhThanh", "MaTinhThanh", phuongXa.MaTinhThanh);
            return View(phuongXa);
        }

        // GET: PhuongXas/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phuongXa = await _context.PhuongXas
                .Include(p => p.MaTinhThanhNavigation)
                .FirstOrDefaultAsync(m => m.MaPhuongXa == id);
            if (phuongXa == null)
            {
                return NotFound();
            }

            return View(phuongXa);
        }

        // POST: PhuongXas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var phuongXa = await _context.PhuongXas.FindAsync(id);
            if (phuongXa != null)
            {
                _context.PhuongXas.Remove(phuongXa);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhuongXaExists(string id)
        {
            return _context.PhuongXas.Any(e => e.MaPhuongXa == id);
        }
    }
}
