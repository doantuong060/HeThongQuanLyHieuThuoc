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
    public class LoaiVatTusController : Controller
    {
        private readonly HieuThuocDbContext _context;

        public LoaiVatTusController(HieuThuocDbContext context)
        {
            _context = context;
        }

        // GET: LoaiVatTus
        public async Task<IActionResult> Index()
        {
            return View(await _context.LoaiVatTus.ToListAsync());
        }

        // GET: LoaiVatTus/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loaiVatTu = await _context.LoaiVatTus
                .FirstOrDefaultAsync(m => m.MaLoaiVt == id);
            if (loaiVatTu == null)
            {
                return NotFound();
            }

            return View(loaiVatTu);
        }

        // GET: LoaiVatTus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LoaiVatTus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaLoaiVt,TenLoaiVt,MoTa")] LoaiVatTu loaiVatTu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(loaiVatTu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(loaiVatTu);
        }

        // GET: LoaiVatTus/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loaiVatTu = await _context.LoaiVatTus.FindAsync(id);
            if (loaiVatTu == null)
            {
                return NotFound();
            }
            return View(loaiVatTu);
        }

        // POST: LoaiVatTus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaLoaiVt,TenLoaiVt,MoTa")] LoaiVatTu loaiVatTu)
        {
            if (id != loaiVatTu.MaLoaiVt)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loaiVatTu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoaiVatTuExists(loaiVatTu.MaLoaiVt))
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
            return View(loaiVatTu);
        }

        // GET: LoaiVatTus/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loaiVatTu = await _context.LoaiVatTus
                .FirstOrDefaultAsync(m => m.MaLoaiVt == id);
            if (loaiVatTu == null)
            {
                return NotFound();
            }

            return View(loaiVatTu);
        }

        // POST: LoaiVatTus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var loaiVatTu = await _context.LoaiVatTus.FindAsync(id);
            if (loaiVatTu != null)
            {
                _context.LoaiVatTus.Remove(loaiVatTu);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoaiVatTuExists(string id)
        {
            return _context.LoaiVatTus.Any(e => e.MaLoaiVt == id);
        }
    }
}
