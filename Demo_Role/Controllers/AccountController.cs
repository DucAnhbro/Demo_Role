using Demo_Role.Constants;
using Demo_Role.CustomAuthorization;
using Demo_Role.CustomAuthorization.Requirement;
using Demo_Role.Dto;
using Demo_Role.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Demo_Role.Controllers
{
    public class AccountController : Controller
    {
        EmployessContext _db = new EmployessContext();
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost, ActionName("Register")]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Employe _employe)
        {
            if (ModelState.IsValid)
            {
                var check = _db.Employes.FirstOrDefault(s => s.Email == _employe.Email);
                if (check == null)
                {
                    _db.Employes.Add(_employe);
                    _db.SaveChanges();
                    return RedirectToAction("Login");
                }
                else
                {
                    ViewBag.error = "Email already exists";
                    return View();
                }
            }
            return View();
        }



        [HttpPost, ActionName("Login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            {
                var roleUserAppContext = _db.Employes.Include(u => u.Group);

                var p = roleUserAppContext.ToList();
                var userDetail = p.Where(x => x.Email == email && x.Password == password).FirstOrDefault();
                var userRoleDetail = new EmployeDetail(userDetail.Id,userDetail.UserName);


                userRoleDetail.Roles = (from ur in _db.EmployeRoles
                                        join r in _db.Roles on ur.RoleId equals r.RoleId
                                        where ur.EmployeId == userDetail.Id
                                        select new EmployeRoleDto
                                        {
                                            Id = ur.Id,
                                            Name = r.RoleName,
                                            Action = r.Action,
                                            Controller = r.Controller,
                                            Status = ur.Status
                                        }).ToList();
                var userStr = Newtonsoft.Json.JsonConvert.SerializeObject(userRoleDetail.Roles);

                 if(userDetail != null && userDetail.GroupId == 2)
                {
                    HttpContext.Session.SetString(SessionConstants.EMAIL, userDetail.Email);
                    HttpContext.Session.SetInt32(SessionConstants.EmployeId, userDetail.Id);
                    HttpContext.Session.SetString(SessionConstants.USERROLE, userStr);
                    return RedirectToAction("Index", "Employe");
                }
                if (userDetail != null && userDetail.GroupId == 1)
                {
                    HttpContext.Session.SetString(SessionConstants.EMAIL, email);
                    HttpContext.Session.SetInt32(SessionConstants.EmployeId, userDetail.Id);
                    HttpContext.Session.SetString(SessionConstants.USERROLE, userStr);
                    return RedirectToAction("Index", "Admin");
                }
            }
            return RedirectToAction("Login", "Acount");
        }

        //[HttpPost, ActionName("Login")]
        //public IActionResult Login(string email, string password)
        //{
        //    var UserExit = _db.Employes.Any(x => x.Email == email && x.Password == password);
        //    if (ModelState.IsValid)
        //    {
        //        if (UserExit)
        //        {
        //            HttpContext.Session.SetString("auth", email);

        //            return RedirectToAction("Index", "Employe");
        //        }
        //        if (UserExit)
        //        {
        //            //HttpContext.Session.SetString("auth", email);

        //            return RedirectToAction("Index", "Admin");
        //        }
        //    }  
        //    return RedirectToAction("Login", "Account");
        //}

        public ActionResult Logout()
        {
            HttpContext.SignOutAsync(
           scheme: "DemoSecurityScheme");
            HttpContext.Session.Remove(SessionConstants.EmployeId);
            return RedirectToAction("Login", "Account");
        }
    }
}
