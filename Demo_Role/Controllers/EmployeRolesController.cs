using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Demo_Role.Models;

namespace Demo_Role.Controllers
{
    public class EmployeRolesController : Controller
    {
        private readonly EmployessContext _context;

        public EmployeRolesController(EmployessContext context)
        {
            _context = context;
        }

        // GET: EmployeRoles
        public async Task<IActionResult> Index()
        {
            var employessContext = _context.EmployeRoles.Include(e => e.Employe).Include(e => e.Role);
            return View(await employessContext.ToListAsync());
        }

        // GET: EmployeRoles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.EmployeRoles == null)
            {
                return NotFound();
            }

            var employeRole = await _context.EmployeRoles
                .Include(e => e.Employe)
                .Include(e => e.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employeRole == null)
            {
                return NotFound();
            }

            return View(employeRole);
        }

        // GET: EmployeRoles/Create
        public IActionResult Create()
        {
            ViewData["EmployeId"] = new SelectList(_context.Employes, "Id", "Id");
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId");
            return View();
        }

        // POST: EmployeRoles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EmployeId,RoleId,Status")] EmployeRole employeRole)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employeRole);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeId"] = new SelectList(_context.Employes, "Id", "Id", employeRole.EmployeId);
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId", employeRole.RoleId);
            return View(employeRole);
        }

        // GET: EmployeRoles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.EmployeRoles == null)
            {
                return NotFound();
            }

            var employeRole = await _context.EmployeRoles.FindAsync(id);
            if (employeRole == null)
            {
                return NotFound();
            }
            ViewData["EmployeId"] = new SelectList(_context.Employes, "Id", "Id", employeRole.EmployeId);
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId", employeRole.RoleId);
            return View(employeRole);
        }

        // POST: EmployeRoles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EmployeId,RoleId,Status")] EmployeRole employeRole)
        {
            if (id != employeRole.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employeRole);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeRoleExists(employeRole.Id))
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
            ViewData["EmployeId"] = new SelectList(_context.Employes, "Id", "Id", employeRole.EmployeId);
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId", employeRole.RoleId);
            return View(employeRole);
        }

        // GET: EmployeRoles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.EmployeRoles == null)
            {
                return NotFound();
            }

            var employeRole = await _context.EmployeRoles
                .Include(e => e.Employe)
                .Include(e => e.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employeRole == null)
            {
                return NotFound();
            }

            return View(employeRole);
        }

        // POST: EmployeRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.EmployeRoles == null)
            {
                return Problem("Entity set 'EmployessContext.EmployeRoles'  is null.");
            }
            var employeRole = await _context.EmployeRoles.FindAsync(id);
            if (employeRole != null)
            {
                _context.EmployeRoles.Remove(employeRole);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeRoleExists(int id)
        {
          return _context.EmployeRoles.Any(e => e.Id == id);
        }
    }
}
