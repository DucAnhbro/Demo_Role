using Demo_Role.Constants;
using Demo_Role.CustomAuthorization;
using Demo_Role.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo_Role.Controllers
{
    public class EmployeController : Controller
    {
        [CustomAuthorize("Employe.Index")]
        public IActionResult Index()
        {
            return View();
        }
        private EmployessContext _db = new EmployessContext();



        [HttpGet]
        [CustomAuthorize("Employe.ShowDetail")]
        public async Task<IActionResult> ShowDetail()
        {
            var userEmail = HttpContext.Session.GetString(SessionConstants.EMAIL);
            if (string.IsNullOrEmpty(SessionConstants.EMAIL) || _db.Employes == null)
            {
                return NotFound();
            }

            var em = await _db.Employes
                .FirstOrDefaultAsync(m => m.Email == userEmail);
            if (em == null)
            {
                return NotFound();
            }


            return View(em);
        }


        [CustomAuthorize("Employe.Edit")]
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
        [CustomAuthorize("Employe.Edit")]
        public async Task<IActionResult> Edit(Employe employe)
        {
            var userEmail = HttpContext.Session.GetString(SessionConstants.EMAIL);
            if (userEmail == null)
            {
                return NotFound();
            }
            //if (id != employe.Id)
            //{
            //    return NotFound();
            //}
            if (userEmail != null){
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
                return RedirectToAction(nameof(Index));
            } 
            return View(employe);
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
