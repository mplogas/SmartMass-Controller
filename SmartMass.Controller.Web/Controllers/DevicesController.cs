using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmartMass.Controller.Model.DTOs;
using SmartMass.Controller.Mqtt;
using SmartMass.Controller.Web.Data;

namespace SmartMass.Controller.Web.Controllers
{
    public class DevicesController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly SmartMassDbContext dbContext;
        private readonly ILogger<DevicesController> logger;
        private readonly IMqttClient mqttClient;

        public DevicesController(ILogger<DevicesController> logger, SmartMassDbContext dbContext, Mqtt.IMqttClient mqttClient)
        {
            this.logger = logger;
            this.dbContext = dbContext;
            this.mqttClient = mqttClient;
        }

        // GET: Devices
        public async Task<IActionResult> Index()
        {
              return dbContext.Devices != null ? 
                          View(await dbContext.Devices.ToListAsync()) :
                          Problem("Entity set 'SmartMassDbContext.Devices'  is null.");
        }

        // GET: Devices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || dbContext.Devices == null)
            {
                return NotFound();
            }

            var device = await dbContext.Devices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (device == null)
            {
                return NotFound();
            }

            return View(device);
        }

        // GET: Devices/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Devices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,CalibrationFactor,ScaleUpdateInterval,ScaleSamplingSize,ScaleCalibrationWeight,ScaleDisplayTimeout")] DeviceDTO deviceDto)
        {
            if (ModelState.IsValid)
            {
                dbContext.Add(deviceDto);
                await dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(deviceDto);
        }

        // GET: Devices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || dbContext.Devices == null)
            {
                return NotFound();
            }

            var device = await dbContext.Devices.FindAsync(id);
            if (device == null)
            {
                return NotFound();
            }
            return View(device);
        }

        // POST: Devices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CalibrationFactor,ScaleUpdateInterval,ScaleSamplingSize,ScaleCalibrationWeight,ScaleDisplayTimeout")] DeviceDTO deviceDto)
        {
            if (id != deviceDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    dbContext.Update(deviceDto);
                    await dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeviceExists(deviceDto.Id))
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
            return View(deviceDto);
        }

        // GET: Devices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || dbContext.Devices == null)
            {
                return NotFound();
            }

            var device = await dbContext.Devices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (device == null)
            {
                return NotFound();
            }

            return View(device);
        }

        // POST: Devices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (dbContext.Devices == null)
            {
                return Problem("Entity set 'SmartMassDbContext.Devices'  is null.");
            }
            var device = await dbContext.Devices.FindAsync(id);
            if (device != null)
            {
                dbContext.Devices.Remove(device);
            }
            
            await dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeviceExists(int id)
        {
          return (dbContext.Devices?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
