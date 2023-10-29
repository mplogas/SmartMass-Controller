﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmartMass.Controller.Model.DTOs;
using SmartMass.Controller.Web.Data;

namespace SmartMass.Controller.Web.Controllers
{
    public class InventoryController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly SmartMassDbContext _context;

        public InventoryController(SmartMassDbContext context)
        {
            _context = context;
        }

        // GET: Inventory
        public async Task<IActionResult> Index()
        {
            var smartMassDbContext = _context.Spools.Include(s => s.ManufacturerDto).Include(s => s.MaterialDto);
            return View(await smartMassDbContext.ToListAsync());
        }

        // GET: Inventory/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Spools == null)
            {
                return NotFound();
            }

            var spool = await _context.Spools
                .Include(s => s.ManufacturerDto)
                .Include(s => s.MaterialDto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (spool == null)
            {
                return NotFound();
            }

            return View(spool);
        }

        // GET: Inventory/Create
        public IActionResult Create()
        {
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "Id", "Name");
            ViewData["MaterialId"] = new SelectList(_context.Materials, "Id", "Type");
            return View();
        }

        // POST: Inventory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Created,EmptySpoolWeight,ManufacturerId,MaterialId,Color,NozzleTemp,BedTemp")] SpoolDTO spoolDto)
        {
            //setting up some stuff to satisfy the validator
            //TODO: viewmodels and stuff. 
            spoolDto.Id = Guid.NewGuid();
            spoolDto.Created = DateTime.UtcNow;
            spoolDto.ManufacturerDto = _context.Manufacturers.Single(m => m.Id == spoolDto.ManufacturerId);
            spoolDto.MaterialDto = _context.Materials.Single(m => m.Id == spoolDto.MaterialId);
            ModelState.ClearValidationState(nameof(ManufacturerDTO));
            TryValidateModel(spoolDto.ManufacturerDto, nameof(ManufacturerDTO));
            ModelState.ClearValidationState(nameof(MaterialDTO));
            TryValidateModel(spoolDto.MaterialDto, nameof(MaterialDTO));
            ModelState.ClearValidationState(nameof(SpoolDTO));
            TryValidateModel(spoolDto, nameof(SpoolDTO));

            if (ModelState.IsValid)
            {
                _context.Add(spoolDto);
                var result = await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var number = ModelState.ErrorCount;
            }
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "Id", "Name", spoolDto.ManufacturerId);
            ViewData["MaterialId"] = new SelectList(_context.Materials, "Id", "Type", spoolDto.MaterialId);
            return View(spoolDto);
        }

        // GET: Inventory/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Spools == null)
            {
                return NotFound();
            }

            var spool = await _context.Spools.FindAsync(id);
            if (spool == null)
            {
                return NotFound();
            }
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "Id", "Name", spool.ManufacturerId);
            ViewData["MaterialId"] = new SelectList(_context.Materials, "Id", "Type", spool.MaterialId);
            return View(spool);
        }

        // POST: Inventory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Created,EmptySpoolWeight,ManufacturerId,MaterialId,Color,NozzleTemp,BedTemp")] SpoolDTO spoolDto)
        {
            if (id != spoolDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(spoolDto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpoolExists(spoolDto.Id))
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
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "Id", "Name", spoolDto.ManufacturerId);
            ViewData["MaterialId"] = new SelectList(_context.Materials, "Id", "Type", spoolDto.MaterialId);
            return View(spoolDto);
        }

        // GET: Inventory/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Spools == null)
            {
                return NotFound();
            }

            var spool = await _context.Spools
                .Include(s => s.ManufacturerDto)
                .Include(s => s.MaterialDto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (spool == null)
            {
                return NotFound();
            }

            return View(spool);
        }

        // POST: Inventory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Spools == null)
            {
                return Problem("Entity set 'SmartMassDbContext.Spools'  is null.");
            }
            var spool = await _context.Spools.FindAsync(id);
            if (spool != null)
            {
                _context.Spools.Remove(spool);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SpoolExists(Guid id)
        {
          return (_context.Spools?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
