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
    public class ThuocsController : Controller
    {
        private readonly HieuThuocDbContext _context;

        public ThuocsController(HieuThuocDbContext context)
        {
            _context = context;
        }

        // GET: Thuocs
        public async Task<IActionResult> Index()
        {
            var hieuThuocDbContext = _context.Thuocs.Include(t => t.MaLoaiNavigation).Include(t => t.MaSpNavigation);
            return View(await hieuThuocDbContext.ToListAsync());
        }

        // GET: Thuocs/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thuoc = await _context.Thuocs
                .Include(t => t.MaLoaiNavigation)
                .Include(t => t.MaSpNavigation)
                .FirstOrDefaultAsync(m => m.MaSp == id);
            if (thuoc == null)
            {
                return NotFound();
            }

            return View(thuoc);
        }

        // GET: Thuocs/Create
        public IActionResult Create()
        {
            ViewData["MaLoai"] = new SelectList(_context.LoaiThuocs, "MaLoai", "MaLoai");
            ViewData["MaSp"] = new SelectList(_context.SanPhams, "MaSp", "MaSp");
            return View();
        }

        // POST: Thuocs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaSp,MaLoai,CanToa,GhiChu")] Thuoc thuoc)
        {
            if (ModelState.IsValid)
            {
                _context.Add(thuoc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaLoai"] = new SelectList(_context.LoaiThuocs, "MaLoai", "MaLoai", thuoc.MaLoai);
            ViewData["MaSp"] = new SelectList(_context.SanPhams, "MaSp", "MaSp", thuoc.MaSp);
            return View(thuoc);
        }

        // GET: Thuocs/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thuoc = await _context.Thuocs.FindAsync(id);
            if (thuoc == null)
            {
                return NotFound();
            }
            ViewData["MaLoai"] = new SelectList(_context.LoaiThuocs, "MaLoai", "MaLoai", thuoc.MaLoai);
            ViewData["MaSp"] = new SelectList(_context.SanPhams, "MaSp", "MaSp", thuoc.MaSp);
            return View(thuoc);
        }

        // POST: Thuocs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaSp,MaLoai,CanToa,GhiChu")] Thuoc thuoc)
        {
            if (id != thuoc.MaSp)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(thuoc);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ThuocExists(thuoc.MaSp))
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
            ViewData["MaLoai"] = new SelectList(_context.LoaiThuocs, "MaLoai", "MaLoai", thuoc.MaLoai);
            ViewData["MaSp"] = new SelectList(_context.SanPhams, "MaSp", "MaSp", thuoc.MaSp);
            return View(thuoc);
        }

        // GET: Thuocs/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thuoc = await _context.Thuocs
                .Include(t => t.MaLoaiNavigation)
                .Include(t => t.MaSpNavigation)
                .FirstOrDefaultAsync(m => m.MaSp == id);
            if (thuoc == null)
            {
                return NotFound();
            }

            return View(thuoc);
        }

        // POST: Thuocs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var thuoc = await _context.Thuocs.FindAsync(id);
            if (thuoc != null)
            {
                _context.Thuocs.Remove(thuoc);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ThuocExists(string id)
        {
            return _context.Thuocs.Any(e => e.MaSp == id);
        }
    }
}
