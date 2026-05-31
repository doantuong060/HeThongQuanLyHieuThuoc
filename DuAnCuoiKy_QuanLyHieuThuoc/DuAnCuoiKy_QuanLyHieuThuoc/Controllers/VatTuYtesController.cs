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
    public class VatTuYtesController : Controller
    {
        private readonly HieuThuocDbContext _context;

        public VatTuYtesController(HieuThuocDbContext context)
        {
            _context = context;
        }

        // GET: VatTuYtes
        public async Task<IActionResult> Index()
        {
            var hieuThuocDbContext = _context.VatTuYtes.Include(v => v.MaLoaiVtNavigation).Include(v => v.MaSpNavigation);
            return View(await hieuThuocDbContext.ToListAsync());
        }

        // GET: VatTuYtes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vatTuYte = await _context.VatTuYtes
                .Include(v => v.MaLoaiVtNavigation)
                .Include(v => v.MaSpNavigation)
                .FirstOrDefaultAsync(m => m.MaSp == id);
            if (vatTuYte == null)
            {
                return NotFound();
            }

            return View(vatTuYte);
        }

        // GET: VatTuYtes/Create
        public IActionResult Create()
        {
            ViewData["MaLoaiVt"] = new SelectList(_context.LoaiVatTus, "MaLoaiVt", "MaLoaiVt");
            ViewData["MaSp"] = new SelectList(_context.SanPhams, "MaSp", "MaSp");
            return View();
        }

        // POST: VatTuYtes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaSp,MaLoaiVt,NhaSanXuat")] VatTuYte vatTuYte)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vatTuYte);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaLoaiVt"] = new SelectList(_context.LoaiVatTus, "MaLoaiVt", "MaLoaiVt", vatTuYte.MaLoaiVt);
            ViewData["MaSp"] = new SelectList(_context.SanPhams, "MaSp", "MaSp", vatTuYte.MaSp);
            return View(vatTuYte);
        }

        // GET: VatTuYtes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vatTuYte = await _context.VatTuYtes.FindAsync(id);
            if (vatTuYte == null)
            {
                return NotFound();
            }
            ViewData["MaLoaiVt"] = new SelectList(_context.LoaiVatTus, "MaLoaiVt", "MaLoaiVt", vatTuYte.MaLoaiVt);
            ViewData["MaSp"] = new SelectList(_context.SanPhams, "MaSp", "MaSp", vatTuYte.MaSp);
            return View(vatTuYte);
        }

        // POST: VatTuYtes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaSp,MaLoaiVt,NhaSanXuat")] VatTuYte vatTuYte)
        {
            if (id != vatTuYte.MaSp)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vatTuYte);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VatTuYteExists(vatTuYte.MaSp))
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
            ViewData["MaLoaiVt"] = new SelectList(_context.LoaiVatTus, "MaLoaiVt", "MaLoaiVt", vatTuYte.MaLoaiVt);
            ViewData["MaSp"] = new SelectList(_context.SanPhams, "MaSp", "MaSp", vatTuYte.MaSp);
            return View(vatTuYte);
        }

        // GET: VatTuYtes/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vatTuYte = await _context.VatTuYtes
                .Include(v => v.MaLoaiVtNavigation)
                .Include(v => v.MaSpNavigation)
                .FirstOrDefaultAsync(m => m.MaSp == id);
            if (vatTuYte == null)
            {
                return NotFound();
            }

            return View(vatTuYte);
        }

        // POST: VatTuYtes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var vatTuYte = await _context.VatTuYtes.FindAsync(id);
            if (vatTuYte != null)
            {
                _context.VatTuYtes.Remove(vatTuYte);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VatTuYteExists(string id)
        {
            return _context.VatTuYtes.Any(e => e.MaSp == id);
        }
    }
}
