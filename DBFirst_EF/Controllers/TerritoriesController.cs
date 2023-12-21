﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DBFirst_EF.Models;

namespace DBFirst_EF.Controllers
{
    public class TerritoriesController : Controller
    {
        private readonly NorthwindContext _context;

        public TerritoriesController(NorthwindContext context)
        {
            _context = context;
        }

        // GET: Territories
        public async Task<IActionResult> Index()
        {
            var data = _context.Territories.Include(t => t.Region); // territories <=> region

            return View(await data.ToListAsync());
        }

        // GET: Territories/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Territories == null)
            {
                return NotFound();
            }

            var territory = await _context.Territories
                .Include(t => t.Region)
                .FirstOrDefaultAsync(m => m.TerritoryId == id);
            if (territory == null)
            {
                return NotFound();
            }

            return View(territory);
        }

        // GET: Territories/Create
        // DB tarafından gelen veri sadece gösteriliyor
        public IActionResult Create()
        {
            ViewData["RegionId"] = new SelectList(_context.Regions, "RegionId", "RegionId");
            return View();
        }

        // POST: Territories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TerritoryId,TerritoryDescription,RegionId")] Territory territory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(territory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RegionId"] = new SelectList(_context.Regions, "RegionId", "RegionId", territory.RegionId);
            return View(territory);
        }

        // GET: Territories/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Territories == null)
            {
                return NotFound();
            }

            var territory = await _context.Territories.FindAsync(id);
            if (territory == null)
            {
                return NotFound();
            }
            ViewData["RegionId"] = new SelectList(_context.Regions, "RegionId", "RegionId", territory.RegionId);
            return View(territory);
        }

        // POST: Territories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("TerritoryId,TerritoryDescription,RegionId")] Territory territory)
        {
            if (id != territory.TerritoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid) // Post edilen bilgiler benim modelime uygunmu
            {
                try
                {
                    _context.Update(territory); // db tablosunun update kısmını
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TerritoryExists(territory.TerritoryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index"); // işlem bitince beni ismi Index olan sayfaya yonlendir
            }
            ViewData["RegionId"] = new SelectList(_context.Regions, "RegionId", "RegionId", territory.RegionId);
            return View(territory);
        }

        // GET: Territories/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Territories == null)
            {
                return NotFound();
            }

            var territory = await _context.Territories
                .Include(t => t.Region)
                .FirstOrDefaultAsync(m => m.TerritoryId == id);
            if (territory == null)
            {
                return NotFound();
            }

            return View(territory);
        }

        // POST: Territories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Territories == null)
            {
                return Problem("Entity set 'NorthwindContext.Territories'  is null.");
            }
            var territory = await _context.Territories.FindAsync(id);
            if (territory != null)
            {
                _context.Territories.Remove(territory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TerritoryExists(string id)
        {
          return (_context.Territories?.Any(e => e.TerritoryId == id)).GetValueOrDefault();
        }
    }
}
