using Demo_Role.CustomAuthorization;
using Demo_Role.Dto;
using Demo_Role.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo_Role.Controllers
{

    public class AdminController : Controller
    {
        private EmployessContext _db = new EmployessContext();
        [CustomAuthorize("Admin.Index")]
        public IActionResult Index()
        {
            return View();
        }
        [CustomAuthorize("Admin.List")]
        public IActionResult List()
        {
            return View(_db.Employes.ToList());
        }
        [CustomAuthorize("Admin.Add")]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("Admin.Add")]
        public ActionResult Add(Employe employe)
        {
            if (ModelState.IsValid)
            {
                var check = _db.Employes.FirstOrDefault(s => s.Id == employe.Id);
                if (check == null)
                {
                    _db.Employes.Add(employe);
                    _db.SaveChanges();
                    return RedirectToAction("List");
                }
                else
                {
                    ViewBag.error = "ID exits";
                    return View();
                }


            }
            return View();
        }

        [CustomAuthorize("Admin.Detail")]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null || _db.Employes == null)
            {
                return NotFound();
            }
            var em = await _db.Employes.FirstOrDefaultAsync(m => m.Id == id);
            if (em == null)
            {
                return NotFound();
            }
            return View(em);
        }


        //public async Task<IActionResult> DetailActionEmploye(int? id)
        //{
        //    if (id <= 0 || !_db.Employes.Any())
        //    {
        //        return NotFound();
        //    }
        //    var employe = await _db.Employes.FindAsync(id);
        //    if (employe == null)
        //    {
        //        return NotFound();
        //    }
        //    var employeDetail = new EmployeDetail(employe);


        //    employeDetail.Roles = (from er in _db.EmployeRoles
        //                        join r in _db.Roles on er.RoleId equals r.RoleId
        //                        where er.Id == id
        //                        select new EmployeRoleDto
        //                        {
        //                            Id = er.Id,
        //                            Name = r.RoleName,
        //                            Action = r.Action,
        //                            Controller = r.Controller,
        //                            Status = er.Status
        //                        }).ToList();
        //    return View(employeDetail);
        //}

        [CustomAuthorize("Admin.Edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _db.Employes == null)
            {
                return NotFound();
            }

            var em = await _db.Employes.FindAsync(id)
;
            if (em == null)
            {
                return NotFound();
            }
            return View(em);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("Admin.Edit")]
        public async Task<IActionResult> Edit(int id, Employe employe)
        {
            if (id != employe.Id)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    _db.Employes.Update(employe);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployesExists(employe.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(List));
            }
            //return View(employe);
        }

        [CustomAuthorize("Admin.Delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _db.Employes == null)
            {
                return NotFound();
            }

            var em = await _db.Employes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (em == null)
            {
                return NotFound();
            }

            return View(em);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("Admin.Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_db.Employes == null)
            {
                return Problem("Entity set 'DataUserContext.Usertbls'  is null.");
            }
            var user = await _db.Employes.FindAsync(id)
;
            if (user != null)
            {
                _db.Employes.Remove(user);
            }

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(List));
        }
        private bool EmployesExists(int id)
        {
            return _db.Employes.Any(e => e.Id == id);
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Remove("Id");
            return RedirectToAction("Login", "Account");
        }
    }
}
