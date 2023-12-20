using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Salon.Data;
using Salon.Models;

namespace Salon.Controllers
{
    public class GroupServicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GroupServicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GroupServices
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["GroupName"] = _context.Groups.Select(d => new { id = d.GroupId, GroupName = d.GroupName }).FirstOrDefault(d => d.id == id).GroupName;
            ViewData["IdGroup"] = _context.Groups.Select(d => new { id = d.GroupId, GroupName = d.GroupName }).FirstOrDefault(d => d.id == id).id;

            var applicationDbContext = _context.Services.Where(d => d.GroupId == id).Include(s => s.Group);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: GroupServices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .Include(s => s.Group)
                .FirstOrDefaultAsync(m => m.ServiceId == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // GET: GroupServices/Create
        [Authorize(Roles = "Administrator,Guest")]
        public IActionResult Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["GroupId"] = id;
            return View();
        }

        // POST: GroupServices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator,Guest")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ServiceId,ServiceName,GroupId,ProductionCost,Price,Description")] Service service)
        {
            if (ModelState.IsValid)
            {
                _context.Add(service);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = service.GroupId });
            }
            ViewData["GroupId"] = service.GroupId;
            return View(service);
        }

        // GET: GroupServices/Edit/5
        [Authorize(Roles = "Administrator,Guest")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            ViewData["GroupId"] = new SelectList(_context.Groups, "GroupId", "GroupName", service.GroupId);
            ViewData["IdGroup"] = service.GroupId;
            return View(service);
        }

        // POST: GroupServices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator,Guest")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ServiceId,ServiceName,GroupId,ProductionCost,Price,Description")] Service service)
        {
            if (id != service.ServiceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(service);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceExists(service.ServiceId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = service.GroupId });
            }
            ViewData["GroupId"] = new SelectList(_context.Groups, "GroupId", "GroupName", service.GroupId);
            ViewData["IdGroup"] = service.GroupId;
            return View(service);
        }

        // GET: GroupServices/Delete/5
        [Authorize(Roles = "Administrator,Guest")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .Include(s => s.Group)
                .FirstOrDefaultAsync(m => m.ServiceId == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // POST: GroupServices/Delete/5
        [Authorize(Roles = "Administrator,Guest")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var service = await _context.Services.FindAsync(id);
            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = service.GroupId });
        }

        private bool ServiceExists(int id)
        {
          return (_context.Services?.Any(e => e.ServiceId == id)).GetValueOrDefault();
        }
    }
}
