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
    public class TinhThanhsController : Controller
    {
        private readonly HieuThuocDbContext _context;

        public TinhThanhsController(HieuThuocDbContext context)
        {
            _context = context;
        }

        // GET: TinhThanhs
        public async Task<IActionResult> Index()
        {
            return View(await _context.TinhThanhs.ToListAsync());
        }

        // GET: TinhThanhs/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tinhThanh = await _context.TinhThanhs
                .FirstOrDefaultAsync(m => m.MaTinhThanh == id);
            if (tinhThanh == null)
            {
                return NotFound();
            }

            return View(tinhThanh);
        }

        // GET: TinhThanhs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TinhThanhs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaTinhThanh,TenTinhThanh")] TinhThanh tinhThanh)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tinhThanh);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tinhThanh);
        }

        // GET: TinhThanhs/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tinhThanh = await _context.TinhThanhs.FindAsync(id);
            if (tinhThanh == null)
            {
                return NotFound();
            }
            return View(tinhThanh);
        }

        // POST: TinhThanhs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaTinhThanh,TenTinhThanh")] TinhThanh tinhThanh)
        {
            if (id != tinhThanh.MaTinhThanh)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tinhThanh);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TinhThanhExists(tinhThanh.MaTinhThanh))
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
            return View(tinhThanh);
        }

        // GET: TinhThanhs/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tinhThanh = await _context.TinhThanhs
                .FirstOrDefaultAsync(m => m.MaTinhThanh == id);
            if (tinhThanh == null)
            {
                return NotFound();
            }

            return View(tinhThanh);
        }

        // POST: TinhThanhs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var tinhThanh = await _context.TinhThanhs.FindAsync(id);
            if (tinhThanh != null)
            {
                _context.TinhThanhs.Remove(tinhThanh);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TinhThanhExists(string id)
        {
            return _context.TinhThanhs.Any(e => e.MaTinhThanh == id);
        }
    }
}
