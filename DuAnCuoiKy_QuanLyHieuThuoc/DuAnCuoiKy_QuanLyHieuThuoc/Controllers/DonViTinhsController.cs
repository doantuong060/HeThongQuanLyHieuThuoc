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
    public class DonViTinhsController : Controller
    {
        private readonly HieuThuocDbContext _context;

        public DonViTinhsController(HieuThuocDbContext context)
        {
            _context = context;
        }

        // GET: DonViTinhs
        public async Task<IActionResult> Index()
        {
            return View(await _context.DonViTinhs.ToListAsync());
        }

        // GET: DonViTinhs/Details/5
        public async Task<IActionResult> Details(byte? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donViTinh = await _context.DonViTinhs
                .FirstOrDefaultAsync(m => m.MaDvt == id);
            if (donViTinh == null)
            {
                return NotFound();
            }

            return View(donViTinh);
        }

        // GET: DonViTinhs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DonViTinhs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaDvt,TenDvt")] DonViTinh donViTinh)
        {
            if (ModelState.IsValid)
            {
                _context.Add(donViTinh);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(donViTinh);
        }

        // GET: DonViTinhs/Edit/5
        public async Task<IActionResult> Edit(byte? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donViTinh = await _context.DonViTinhs.FindAsync(id);
            if (donViTinh == null)
            {
                return NotFound();
            }
            return View(donViTinh);
        }

        // POST: DonViTinhs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(byte id, [Bind("MaDvt,TenDvt")] DonViTinh donViTinh)
        {
            if (id != donViTinh.MaDvt)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(donViTinh);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DonViTinhExists(donViTinh.MaDvt))
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
            return View(donViTinh);
        }

        // GET: DonViTinhs/Delete/5
        public async Task<IActionResult> Delete(byte? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donViTinh = await _context.DonViTinhs
                .FirstOrDefaultAsync(m => m.MaDvt == id);
            if (donViTinh == null)
            {
                return NotFound();
            }

            return View(donViTinh);
        }

        // POST: DonViTinhs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(byte id)
        {
            var donViTinh = await _context.DonViTinhs.FindAsync(id);
            if (donViTinh != null)
            {
                _context.DonViTinhs.Remove(donViTinh);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DonViTinhExists(byte id)
        {
            return _context.DonViTinhs.Any(e => e.MaDvt == id);
        }
    }
}
