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
    public class LoaiThuocsController : Controller
    {
        private readonly HieuThuocDbContext _context;

        public LoaiThuocsController(HieuThuocDbContext context)
        {
            _context = context;
        }

        // GET: LoaiThuocs
        public async Task<IActionResult> Index()
        {
            return View(await _context.LoaiThuocs.ToListAsync());
        }

        // GET: LoaiThuocs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loaiThuoc = await _context.LoaiThuocs
                .FirstOrDefaultAsync(m => m.MaLoai == id);
            if (loaiThuoc == null)
            {
                return NotFound();
            }

            return View(loaiThuoc);
        }

        // GET: LoaiThuocs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LoaiThuocs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaLoai,TenLoai,MoTa")] LoaiThuoc loaiThuoc)
        {
            if (ModelState.IsValid)
            {
                _context.Add(loaiThuoc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(loaiThuoc);
        }

        // GET: LoaiThuocs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loaiThuoc = await _context.LoaiThuocs.FindAsync(id);
            if (loaiThuoc == null)
            {
                return NotFound();
            }
            return View(loaiThuoc);
        }

        // POST: LoaiThuocs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaLoai,TenLoai,MoTa")] LoaiThuoc loaiThuoc)
        {
            if (id != loaiThuoc.MaLoai)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loaiThuoc);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoaiThuocExists(loaiThuoc.MaLoai))
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
            return View(loaiThuoc);
        }

        // GET: LoaiThuocs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loaiThuoc = await _context.LoaiThuocs
                .FirstOrDefaultAsync(m => m.MaLoai == id);
            if (loaiThuoc == null)
            {
                return NotFound();
            }

            return View(loaiThuoc);
        }

        // POST: LoaiThuocs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loaiThuoc = await _context.LoaiThuocs.FindAsync(id);
            if (loaiThuoc != null)
            {
                _context.LoaiThuocs.Remove(loaiThuoc);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoaiThuocExists(int id)
        {
            return _context.LoaiThuocs.Any(e => e.MaLoai == id);
        }
    }
}
